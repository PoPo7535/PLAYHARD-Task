using System;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

public class GameStepManager : LocalSingleton<GameStepManager>
{
    public BubbleShooter shooter;
    public BubbleEnergy energy;
    public Boss boss;
    [SerializeField] private GameObject _hole;
    public Vector3 holePos
    {
        get
        {
            var pos = _hole.transform.position;
            var offy =Random.Range(-0.1f, 0.1f);
            var offx =Random.Range(-0.7f, 0.7f);

            return new Vector3(offx, offy, 0) + pos;
        }
    }

    private GameStepType _currentStep;
    private IGameStep _aimStep;
    private IGameStep _bossStep;
    private IGameStep _bubbleFallStep;
    private IGameStep _bubbleRefillStep;

    private void Start()
    {
        var aimStep = new AimStep();
        _aimStep = aimStep;
        
        var bubbleFallStep = new BubbleFallStep();
        _bubbleFallStep = bubbleFallStep;
        
        var bossStep = new BossStep();
        _bossStep = bossStep;
        
        var refillStep = new BubbleRefillStep();
        _bubbleRefillStep = refillStep;
        
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
            GameStepType.BubbleRefill => _bubbleRefillStep,
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

    public void ChangeNextStep()
    {
        var newType = _currentStep switch
        {
            GameStepType.Aim => GameStepType.BubbleFall,
            GameStepType.BubbleFall => GameStepType.Boss,
            GameStepType.Boss => GameStepType.BubbleRefill,
            GameStepType.BubbleRefill => GameStepType.Aim,
            _ => GameStepType.Aim
        };
        ChangeStep(newType);
    }
}

public enum GameStepType
{
    Aim,
    BubbleFall,
    Boss,
    BubbleRefill,
}
