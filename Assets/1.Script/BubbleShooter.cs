using System;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public partial class BubbleShooter : MonoBehaviour
{
    [NonSerialized] public Bubble predictionBubble;
    [NonSerialized] public RaycastHit2D[] hit = new RaycastHit2D[2];
    [NonSerialized] public bool activeAim = true;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private LineParticle lineParticle;
    [SerializeField] private float viewDis = 10f;
    [SerializeField] private float viewAngle = 90f;
    public float shootSpeed = 5f;
    
    public int bubbleCount = 22;
    public void Start()
    {
        InitPredictionBubble();
        InitBubbles();
    }


    private void InitPredictionBubble()
    {
        predictionBubble = ObjectPoolManager.I.BubblePool.Get();
        predictionBubble.tag = "Untagged";
        predictionBubble.gameObject.SetActive(false);
        predictionBubble.SetType(BubbleType.Bule);
        predictionBubble.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 50);
        predictionBubble.GetComponent<CircleCollider2D>().enabled = false;
        predictionBubble.transform.parent = transform;
    }

    public void ShooterTrajectory()
    {
        // 각도 계산
        predictionBubble.SetType(CurrentBubble);
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
        if (hit.IsUnityNull()) 
            return;

        // 예측 샷
        if (hit[1].transform.CompareTag("Bubble"))
            predictionBubble.transform.position = HexagonGrid.I.GetPosToWorldPos(hit[1].point);
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

    public void SetVisualsActive(bool isActive)
    {
        lineParticle.gameObject.SetActive(isActive);
        predictionBubble.gameObject.SetActive(isActive);   
    }


#if UNITY_EDITOR
    private readonly int _segments = 5;
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
