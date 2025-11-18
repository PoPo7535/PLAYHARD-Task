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
        var bubbleList = HexagonGrid.I.CollectConnectedBubbles(cell);
        if (3 <= bubbleList.Count)
        {
            foreach (var bubble in bubbleList)
                bubble.Drop();
        }
        HexagonGrid.I.FindDropBubble(new Vector2Int[] { new(3, 4), new(7, 4) });
        await Task.Delay(3000);
        
        _shooter.activeAim = true;
        GameStepManager.I.ChangeStep(GameStepType.Aim);
    }
    public void Exit() { }
}
