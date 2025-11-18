using UnityEngine;

public class BossStep : IGameStep
{
    private Boss _boss;
    public void Init(Boss boss)
    {
        _boss = boss;
    }
    public void GameSteUpdate() { }

    public void Enter()
    {
        
    }
    public void Exit() { }
}
