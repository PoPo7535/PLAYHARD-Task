using UnityEngine;

public class BubbleRefillStep : IGameStep
{
    private BubbleShooter Shooter => GameStepManager.I.shooter;
    public void GameSteUpdate() { }

    public async void Enter()
    {
        await Shooter.RefillBubble();
        if (Shooter.NoBubbles())
            GameStepManager.I.ChangeStep(GameStepType.GameEndStpe);
        else
            GameStepManager.I.ChangeNextStep();
    }
    public void Exit() { }
}
