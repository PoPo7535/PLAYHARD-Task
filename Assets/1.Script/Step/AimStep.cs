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
        if (false == _shooter.activeAim)
            return;
        if (Input.GetMouseButtonUp(0) ||
            Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            if (_shooter.predictionBubble.gameObject.activeSelf)
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
        _shooter.activeAim = false;
        var bubble = BubblePool.I.Pool.Get();
        bubble.transform.position = _shooter.transform.position;
        bubble.SetType(_shooter.CurrentBubble);
        bubble.transform.DOMove(HexagonGrid.I.GetPosToWorldPos(_shooter.hit[0].point), _shooter.shootSpeed)
            .SetSpeedBased()
            .SetEase(Ease.Linear).
            OnComplete(() =>
            {
                bubble.transform.DOMove(_shooter.predictionBubble.transform.position, _shooter.shootSpeed)
                    .SetEase(Ease.Linear)
                    .SetSpeedBased().OnComplete(() =>
                    {
                        BubblePool.I.Pool.Release(bubble);
                        GameStepManager.I.ChangeStep(GameStepType.BubbleFall);
                    });
            });
    }
    public void Enter() { }
    public void Exit() { }
}
