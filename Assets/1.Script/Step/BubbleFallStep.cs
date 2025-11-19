using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class BubbleFallStep : IGameStep
{
    private readonly float _dropDur = 2f;
    private readonly float _off = 0.1f;
    public void Init(BubbleShooter shooter)
    {
        _shooter = shooter;
    }

    private BubbleShooter _shooter;
    public void GameSteUpdate() { }

    public async void Enter()
    {
        var cell = _shooter._predictionBubble.Cell;
        var dropCheck1 = 0f;
        if (_shooter._predictionBubble.MyType == BubbleType.Energy)
            dropCheck1 = HexagonGrid.I.EnergyPopBubbles(cell);
        else
            dropCheck1 = HexagonGrid.I.ConnectedDropBubbles(cell, _dropDur, _off);
        var dropCheck2= HexagonGrid.I.FindDropBubbles(new Vector2Int[] { new(3, 4), new(7, 4) }, _dropDur, _off);
        if (1 <= dropCheck1 || 1 <= dropCheck2)
        {
            var maxDur = Mathf.Max(dropCheck1, dropCheck2);
            await Task.Delay((int)(maxDur * 1000));
        }
        _shooter.activeControll = true;
        GameStepManager.I.ChangeNextStep();
    }
    public void Exit() { }
}
