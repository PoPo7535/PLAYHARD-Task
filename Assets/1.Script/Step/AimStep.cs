using DG.Tweening;
using UnityEngine;

public class AimStep : IGameStep
{
    private BubbleShooter Shooter => GameStepManager.I.shooter;

    public void GameSteUpdate()
    {
        if (false == Shooter.activeControll)
            return;
        if (Input.GetMouseButtonUp(0) ||
            Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            if (Shooter._predictionBubble.gameObject.activeSelf)
                BubbleShot();
            Shooter.SetVisualsActive(false);
        }

        if (Input.GetMouseButtonDown(0) ||
            Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Shooter.ShooterTrajectory();
        }
        
        if (Input.GetMouseButton(0) ||
            Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Shooter.ShooterTrajectory();
        }
    }

    private void BubbleShot()
    {
        Shooter.activeControll = false;
        var bubble = ObjectPoolManager.I.BubblePool.Get();
        bubble.transform.position = Shooter.transform.position;
        bubble.SetType(Shooter.CurrentBubbleType);
        Shooter.SetHandBubbleType(0, BubbleType.None);
        bubble.transform.DOMove(HexagonGrid.I.GetPosToWorldPos(Shooter._hit[0].point), Shooter.shootSpeed)
            .SetSpeedBased()
            .SetEase(Ease.Linear).
            OnComplete(() =>
            {
                bubble.transform.DOMove(Shooter._predictionBubble.transform.position, Shooter.shootSpeed)
                    .SetEase(Ease.Linear)
                    .SetSpeedBased().OnComplete(() =>
                    {
                        var cell = Shooter._predictionBubble.Cell;
                        HexagonGrid.I.SetBubble(bubble, cell, bubble.MyType);
                        GameStepManager.I.ChangeNextStep();
                    });
            });
    }
    public void Enter() { }
    public void Exit() { }
}
