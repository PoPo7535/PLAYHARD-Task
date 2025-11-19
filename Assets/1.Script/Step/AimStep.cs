using DG.Tweening;
using UnityEngine;

public class AimStep : IGameStep
{
    private BubbleShooter _shooter;
    public void Init(BubbleShooter shooter)
    {
        _shooter = shooter;
    }

    public void GameSteUpdate()
    {
        if (false == _shooter._activeControll)
            return;
        if (Input.GetMouseButtonUp(0) ||
            Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            if (_shooter._predictionBubble.gameObject.activeSelf)
                BubbleShot();
            _shooter.SetVisualsActive(false);
        }

        if (Input.GetMouseButtonDown(0) ||
            Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            _shooter.ShooterTrajectory();
        }
        
        if (Input.GetMouseButton(0) ||
            Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            _shooter.ShooterTrajectory();
        }
    }

    private void BubbleShot()
    {
        _shooter._activeControll = false;
        var bubble = ObjectPoolManager.I.BubblePool.Get();
        bubble.transform.position = _shooter.transform.position;
        bubble.SetType(_shooter.CurrentBubbleType);
        _shooter.SetHandBubbleType(0, BubbleType.None);
        bubble.transform.DOMove(HexagonGrid.I.GetPosToWorldPos(_shooter._hit[0].point), _shooter.shootSpeed)
            .SetSpeedBased()
            .SetEase(Ease.Linear).
            OnComplete(() =>
            {
                bubble.transform.DOMove(_shooter._predictionBubble.transform.position, _shooter.shootSpeed)
                    .SetEase(Ease.Linear)
                    .SetSpeedBased().OnComplete(() =>
                    {
                        var cell = _shooter._predictionBubble.Cell;
                        HexagonGrid.I.SetBubble(bubble, cell, bubble.MyType);
                        GameStepManager.I.ChangeNextStep();
                    });
            });
    }
    public void Enter() { }
    public void Exit() { }
}
