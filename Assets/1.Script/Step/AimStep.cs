using DG.Tweening;
using UnityEngine;

public class AimStep : IGameStep
{
    public BubbleShooter shooter;
    public void GameSteUpdate()
    {
        if (false == shooter.activeAim)
            return;
        if (Input.GetMouseButtonUp(0) ||
            Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            if (shooter.predictionBubble.gameObject.activeSelf)
                BubbleShot();
            shooter.SetVisualsActive(false);
        }

        if (Input.GetMouseButtonDown(0) ||
            Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            shooter.ShooterTrajectory();
        }
        
        if (Input.GetMouseButton(0) ||
            Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            shooter.ShooterTrajectory();
        }
    }

    private void BubbleShot()
    {
        shooter.activeAim = false;
        var bubble = BubblePool.I.Pool.Get();
        bubble.transform.position = shooter.transform.position;
        bubble.SetType(BubbleType.Bule);
        bubble.transform.DOMove(HexagonGrid.I.GetPosToWorldPos(shooter.hit[0].point), shooter.shootSpeed)
            .SetSpeedBased()
            .SetEase(Ease.Linear).
            OnComplete(() =>
            {
                bubble.transform.DOMove(shooter.predictionBubble.transform.position, shooter.shootSpeed)
                    .SetEase(Ease.Linear)
                    .SetSpeedBased().OnComplete(() =>
                    {
                        BubblePool.I.Pool.Release(bubble);
                        GameStepManager.I.ChangeStep(GameStepType.BubbleFall);
                        // var cell = HexagonGrid.I.GetPosToCellNumber(shooter.predictionBubble.transform.position);
                        // HexagonGrid.I.SetBubble(bubble, cell, BubbleType.Bule);
                        // var bubbleList = HexagonGrid.I.CollectConnectedBubbles(cell);
                        // if (3 <= bubbleList.Count)
                        // {
                        //     foreach (var bubble in bubbleList)
                        //         bubble.Drop();
                        // }
                        // HexagonGrid.I.FindDropBubble(new Vector2Int[] { new(3, 4), new(7, 4) });
                        // shooter.activeAim = true;
                    });
            });
    }
    public void Enter() { }
    public void Exit() { }
}
