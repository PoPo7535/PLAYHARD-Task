using System;
using DG.Tweening;
using UnityEngine;

public class BubbleFallStep : IGameStep
{
    public BubbleShooter shooter;
    public void GameSteUpdate() { }

    public void Enter()
    {
        var cell = HexagonGrid.I.GetPosToCellNumber(shooter.predictionBubble.transform.position);
        HexagonGrid.I.SetBubble(null, cell, BubbleType.Bule);
        var bubbleList = HexagonGrid.I.CollectConnectedBubbles(cell);
        if (3 <= bubbleList.Count)
        {
            foreach (var bubble in bubbleList)
                bubble.Drop();
        }
        HexagonGrid.I.FindDropBubble(new Vector2Int[] { new(3, 4), new(7, 4) });
        shooter.activeAim = true;
    }
    public void Exit() { }
    

}
