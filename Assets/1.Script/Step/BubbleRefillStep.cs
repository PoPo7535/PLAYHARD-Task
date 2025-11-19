using UnityEngine;

public class BubbleRefillStep : IGameStep
{
    private BubbleShooter _shooter;

    public void Init(BubbleShooter shooter)
    {
        _shooter = shooter;
    }
    public void GameSteUpdate()
    {
        
    }

    public async void Enter()
    {
        await _shooter.RefillBubble();
        GameStepManager.I.ChangeNextStep();

    }

    public void Exit()
    {
    }
}
