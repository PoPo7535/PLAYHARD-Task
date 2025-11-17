using System;
using System.Numerics;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class BubbleShooter : MonoBehaviour, IGameStep
{
    public LineParticle lineParticle;
    public float viewDis = 10f;
    public float viewAngle = 90f;
    public float shootSpeed = 5f;
    public int bubbleCount = 22;
    private int _segments = 5;  

    private Bubble _predictionBubble;
    private RaycastHit2D[] hit = new RaycastHit2D[2];

    public void Start()
    {
        InitPredictionBubble();
        GameStepManager.I.SetStep(GameStepType.Aim, this);
    }
    private void InitPredictionBubble()
    {
        _predictionBubble = BubblePool.I.Pool.Get();
        _predictionBubble.tag = "Untagged";
        _predictionBubble.gameObject.SetActive(false);
        _predictionBubble.SetType(BubbleType.Bule);
        _predictionBubble.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 50);
        _predictionBubble.GetComponent<CircleCollider2D>().enabled = false;
        _predictionBubble.transform.parent = transform;
    }


    public void Update()
    {
        if (Input.GetMouseButtonUp(0) ||
            Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            if (_predictionBubble.gameObject.activeSelf)
            {
                BubbleShot();
            }
            SetVisualsActive(false);
        }

        if (Input.GetMouseButtonDown(0) ||
            Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
             ShooterTrajectory();
        }
        
        if (Input.GetMouseButton(0) ||
            Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            ShooterTrajectory();
        }
    }

    private void BubbleShot()
    {
        var bubble = BubblePool.I.Pool.Get();
        bubble.transform.position = transform.position;
        bubble.SetType(BubbleType.Bule);
        bubble.transform.DOMove(HexagonGrid.I.GetPosToWorldPos(hit[0].point), shootSpeed)
            .SetSpeedBased()
            .SetEase(Ease.Linear).
            OnComplete(() =>
            {
                bubble.transform.DOMove(_predictionBubble.transform.position, shootSpeed)
                    .SetEase(Ease.Linear)
                    .SetSpeedBased().OnComplete(() =>
                    {
                        var cell = HexagonGrid.I.GetPosToCellNumber(_predictionBubble.transform.position);
                        HexagonGrid.I.SetBubble(bubble, cell, BubbleType.Bule);
                        var bubbleList = HexagonGrid.I.CollectConnectedBubbles(cell);
                        if (3 <= bubbleList.Count)
                        {
                            foreach (var bubble in bubbleList)
                            {
                                bubble.Drop();
                            }
                        }

                        HexagonGrid.I.FindDropBubble(new Vector2Int[] { new(3, 4), new(7, 4) });
                    });
            });
    }

    private void ShooterTrajectory()
    {
        // 각도 계산
        var screenPos = Utile.GetPointerWorldPosition();
        var dir = (screenPos - (Vector2)transform.position).normalized;
        if (viewAngle / 2f < Vector2.Angle(transform.up, dir))
        {
            SetVisualsActive(false);
            return;
        }
        SetVisualsActive(true);

        // 예측 궤도
        ShooterTrajectory(dir);
        if (hit.IsUnityNull()) return;

        // 예측 샷
        if (hit[1].transform.CompareTag("Bubble"))
            _predictionBubble.transform.position = HexagonGrid.I.GetPosToWorldPos(hit[1].point);
    }

    private void ShooterTrajectory(Vector2 dir)
    {
        var newHit= Physics2D.CircleCast(transform.position, 0.1f, dir, viewDis);
        newHit = PointOffSet(newHit);
        hit[1] = hit[0] = newHit;
        if (hit[0].transform.CompareTag("Wall"))
        {
            var point = hit[0].centroid - (dir * 0.01f);
            dir = Vector3.Reflect(dir, hit[0].normal);
            newHit = Physics2D.CircleCast(point, 0.1f, dir, viewDis);
            newHit = PointOffSet(newHit);
            hit[1] = newHit;
        }
        lineParticle.SetPosition(0, transform.position);
        lineParticle.SetPosition(1, hit[0].centroid);
        lineParticle.SetPosition(2, hit[1].centroid);
        return;

        RaycastHit2D PointOffSet(RaycastHit2D hit)
        {
            hit.point= new Vector2(hit.point.x, hit.point.y - (HexagonGrid.I.CellSize.y / 2));
            return hit;
        }
    }
    private void SetVisualsActive(bool isActive)
    {
        lineParticle.gameObject.SetActive(isActive);
        _predictionBubble.gameObject.SetActive(isActive);   
    }
    public void GameSteUpdate()
    {
    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }

#if UNITY_EDITOR
    public bool showGizmos = true;
    public void OnDrawGizmos()
    {
        if (false == showGizmos)
            return;
        Gizmos.color = Color.blue;

        Vector2 origin = transform.position;
        var step = viewAngle / _segments;

        // 각도 시작 = -viewAngle/2
        var prevPoint = origin + DirFromAngle(-viewAngle / 2) * viewDis;

        // 중심 → 양 끝 라인
        Gizmos.DrawLine(origin, prevPoint);
        Gizmos.DrawLine(origin, origin + DirFromAngle(viewAngle / 2) * viewDis);

        // 원호 그리기
        for (int i = 1; i <= _segments; ++i)
        {
            var curAngle = -viewAngle / 2 + step * i;
            var nextPoint = origin + DirFromAngle(curAngle) * viewDis;

            Gizmos.DrawLine(prevPoint, nextPoint);

            prevPoint = nextPoint;
            

        }
        return;

        Vector2 DirFromAngle(float angle)
        {
            var rad = (transform.eulerAngles.z + 90f + angle) * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        }
    }
#endif

}
