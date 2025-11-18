using System;
using UnityEngine;
using Utility;

public class GameStepManager : LocalSingleton<GameStepManager>
{
    public BubbleShooter shooter;
    public Boss boss;
    private GameStepType _currentStep;
    private IGameStep _readyStep;
    private IGameStep _aimStep;
    private IGameStep _bossStep;
    private IGameStep _bubbleFallStep;

    private void Start()
    {
        var aimStep = new AimStep();
        aimStep.Init(shooter);
        _aimStep = aimStep;
        
        var bubbleFallStep = new BubbleFallStep();
        bubbleFallStep.Init(shooter);
        _bubbleFallStep = bubbleFallStep;
        
        var bossStep = new BossStep();
        bossStep.Init(boss);
        _bossStep = bossStep;
        
        _currentStep = GameStepType.Aim;
    }

    private void Update()
    {
        switch (_currentStep)
        {
            case GameStepType.Aim:
                _aimStep.GameSteUpdate();
                break;
            case GameStepType.BubbleFall:
                _bubbleFallStep.GameSteUpdate();
                break;
            case GameStepType.Boss:
                _bossStep.GameSteUpdate();
                break;
        }
    }

    private IGameStep GetCurrentStep()
    {
        return _currentStep switch
        {
            GameStepType.Aim => _aimStep,
            GameStepType.BubbleFall => _bubbleFallStep,
            GameStepType.Boss => _bossStep,
            _ => null
        };
    }

    public void ChangeStep(GameStepType type)
    {
        GetCurrentStep().Exit();
        Debug.Log($"게임스텝 변경 {_currentStep} => {type}");
        _currentStep = type;
        GetCurrentStep().Enter();
    }
}

public enum GameStepType
{
    Aim,
    BubbleFall,
    Boss,
}
