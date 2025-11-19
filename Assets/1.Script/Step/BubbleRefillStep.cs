using UnityEngine;

public class BubbleRefillStep : IGameStep
{
    private BubbleShooter Shooter => GameStepManager.I.shooter;
    public void GameSteUpdate() { }

    public async void Enter()
    {
        await Shooter.RefillBubble();
        GameStepManager.I.ChangeNextStep();
    }
    public void Exit() { }
}
