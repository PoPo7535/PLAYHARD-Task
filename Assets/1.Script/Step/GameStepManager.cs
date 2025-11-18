using System;
using UnityEngine;
using Utility;

public class GameStepManager : LocalSingleton<GameStepManager>
{
    public BubbleShooter shooter;
    private GameStepType _currentStep;
    private IGameStep _readyStep;
    private IGameStep _aimStep;
    private IGameStep _fireStep;
    private IGameStep _bubbleFallStep;

    private void Start()
    {
        var aimStep = new AimStep();
        aimStep.Init(shooter);
        _aimStep = aimStep;
        
        var bubbleFallStep = new BubbleFallStep();
        bubbleFallStep.Init(shooter);
        _bubbleFallStep = bubbleFallStep;
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
        }
    }

    private IGameStep GetCurrentStep()
    {
        return _currentStep switch
        {
            GameStepType.Aim => _aimStep,
            GameStepType.BubbleFall => _bubbleFallStep,
            _ => null
        };
    }

    public void ChangeStep(GameStepType type)
    {
        GetCurrentStep().Exit();
        _currentStep = type;
        GetCurrentStep().Enter();
    }
}

public enum GameStepType
{
    Aim,
    BubbleFall,
}
