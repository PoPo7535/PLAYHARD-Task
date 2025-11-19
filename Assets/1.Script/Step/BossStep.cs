using UnityEngine;

public class BossStep : IGameStep
{
    private Boss Boss => GameStepManager.I.boss;

    public void GameSteUpdate() { }

    public async void Enter()
    {
        await Boss.BubbleLineRefill();
        GameStepManager.I.ChangeNextStep();
    }
    public void Exit() { }
}
