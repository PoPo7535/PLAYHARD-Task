using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class BubbleFallStep : IGameStep
{
    public void Init(BubbleShooter shooter)
    {
        _shooter = shooter;
    }

    private BubbleShooter _shooter;
    public void GameSteUpdate() { }

    public async void Enter()
    {
        var cell = HexagonGrid.I.GetPosToCellNumber(_shooter.predictionBubble.transform.position);
        HexagonGrid.I.SetBubble(null, cell, BubbleType.Bule);
        var dropCheck1 = HexagonGrid.I.ConnectedDropBubbles(cell);
        var dropCheck2= HexagonGrid.I.FindDropBubbles(new Vector2Int[] { new(3, 4), new(7, 4) });
        if (dropCheck1 || dropCheck2)
            await Task.Delay(1000);
        
        _shooter.activeAim = true;
        GameStepManager.I.ChangeStep(GameStepType.Boss);
    }
    public void Exit() { }
}
