using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class BubbleFallStep : IGameStep
{
    private float dur = 2f;
    private int Delay => (int)dur * 1000;
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
        var dropCheck1 = HexagonGrid.I.ConnectedDropBubbles(cell, dur);
        var dropCheck2= HexagonGrid.I.FindDropBubbles(new Vector2Int[] { new(3, 4), new(7, 4) }, dur);
        if (dropCheck1 || dropCheck2)
            await Task.Delay(Delay);
        
        _shooter.activeAim = true;
        GameStepManager.I.ChangeStep(GameStepType.Boss);
    }
    public void Exit() { }
}
