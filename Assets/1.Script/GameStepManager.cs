using System;
using UnityEngine;
using Utility;

public class GameStepManager : LocalSingleton<GameStepManager>
{

    private GameStepType _currentStep;
    private IGameStep _readyStep;
    private IGameStep _aimStep;
    private IGameStep _fireStep;
    private IGameStep _bubbleFallStep;

    private void Update()
    {
        switch (_currentStep)
        {
            case GameStepType.Ready:
                _readyStep.GameSteUpdate();
                break;
            case GameStepType.Aim:
                _aimStep.GameSteUpdate();
                break;
            case GameStepType.Fire:
                _fireStep.GameSteUpdate();
                break;
            case GameStepType.BubbleFall:
                _bubbleFallStep.GameSteUpdate();
                break;
        }
    }

    public void SetStep(GameStepType type, IGameStep step)
    {
        switch (type)
        {
            case GameStepType.Ready:
                _readyStep = step;
                break;
            case GameStepType.Aim:
                _aimStep = step;
                break;
            case GameStepType.Fire:
                _fireStep = step;
                break;
            case GameStepType.BubbleFall:
                _bubbleFallStep = step;
                break;
        }
    }
    private IGameStep GetCurrentStep()
    {
        return _currentStep switch
        {
            GameStepType.Ready => _readyStep,
            GameStepType.Aim => _aimStep,
            GameStepType.Fire => _fireStep,
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
    Ready,
    Aim,
    Fire,
    BubbleFall,
}
