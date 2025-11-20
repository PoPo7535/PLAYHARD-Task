using UnityEngine;

public class GameEndStep : IGameStep
{
    
    public void GameSteUpdate()
    {
    }

    public async void Enter()
    {
        HexagonGrid.I.SpareBubblePop();
        await GameStepManager.I.shooter.SpareBubbleToScore();
    }

    public void Exit()
    {
    }
}
