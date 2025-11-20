using System.Threading.Tasks;
using UnityEngine;

public class BubbleFallStep : IGameStep
{
    private readonly float _dropDur = 2f;
    private readonly float _off = 0.07f;
    private BubbleShooter Shooter => GameStepManager.I.shooter;
    public void GameSteUpdate() { }
    public async void Enter()
    {
        var cell = Shooter.predictionBubble.Cell;
        float dropCheck1;
        if (Shooter.predictionBubble.MyType == BubbleType.Energy)
            dropCheck1 = HexagonGrid.I.EnergyPopBubbles(cell, _dropDur, _off);
        else
            dropCheck1 = HexagonGrid.I.ConnectedPopBubbles(cell, _dropDur, _off);
        var dropCheck2= HexagonGrid.I.FindDropBubbles(new Vector2Int[] { new(3, 4), new(7, 4) }, _dropDur, _off);
        if (1 <= dropCheck1 || 1 <= dropCheck2)
        {
            ScoreHelper.AddCombo();
            var maxDur = Mathf.Max(dropCheck1, dropCheck2);
            await Task.Delay((int)(maxDur * 1000));
        }
        else
        {
            ScoreHelper.ReSetCombo();
        }
        Shooter.activeControll = true;
        GameStepManager.I.ChangeNextStep();
    }
    public void Exit() { }
}
