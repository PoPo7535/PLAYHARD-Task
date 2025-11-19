using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class BubbleFallStep : IGameStep
{
    private float dur = 2f;
    private float off = 0.1f;
    private int Delay => (int)(dur * 1000);
    private int OffDelay => (int)(off * 1000);
    public void Init(BubbleShooter shooter)
    {
        _shooter = shooter;
    }

    private BubbleShooter _shooter;
    public void GameSteUpdate() { }

    public async void Enter()
    {
        var cell = _shooter._predictionBubble.Cell;
        var dropCheck1 = HexagonGrid.I.ConnectedDropBubbles(cell, dur, off);
        var dropCheck2= HexagonGrid.I.FindDropBubbles(new Vector2Int[] { new(3, 4), new(7, 4) }, dur, off);
        if (1 <= dropCheck1 || 1 <= dropCheck2)
        {
            var maxCount = Math.Max(dropCheck1, dropCheck2);
            await Task.Delay(Delay + (OffDelay * maxCount));
        }
        _shooter._activeControll = true;
        GameStepManager.I.ChangeNextStep();
    }
    public void Exit() { }
}
