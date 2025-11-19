using UnityEngine;

public class BossStep : IGameStep
{
    private Boss _boss;
    public void Init(Boss boss)
    {
        _boss = boss;
    }
    public void GameSteUpdate() { }

    public async void Enter()
    {
        await _boss.BubbleLineRefill();
        GameStepManager.I.ChangeNextStep();
    }
    public void Exit() { }
}
