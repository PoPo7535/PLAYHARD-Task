using System;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public partial class BubbleShooter : MonoBehaviour
{
    [NonSerialized] public Bubble predictionBubble;
    [NonSerialized] public readonly RaycastHit2D[] hit = new RaycastHit2D[2];
    [NonSerialized] public bool activeControll = true;
    [SerializeField] private SpriteRenderer _sr;
    
    [SerializeField] private LineParticle _lineParticle;
    [SerializeField] private float _viewDis = 10f;
    [SerializeField] private float _viewAngle = 90f;
    
    private Vector3 _shotPos;
    public float shootSpeed = 10f;
    public void Start()
    {
        InitPredictionBubble();
        InitBubbles();
        _shotPos = transform.position + new Vector3(0, _sr.size.y / 2, 0);
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
        predictionBubble.SetType(CurrentBubbleType);
        var screenPos = Utile.GetPointerWorldPosition();
        var dir = (screenPos - (Vector2)_shotPos);
        var distance = dir.magnitude;
        dir.Normalize();
        if (_viewAngle / 2f < Vector2.Angle(transform.up, dir) ||
            distance < _sr.size.x / 2.1f)
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
        var layerMask = ~LayerMask.GetMask("Ignore Raycast");
        var newHit = Physics2D.CircleCast(_shotPos, 0.1f, dir, _viewDis, layerMask: layerMask);
        newHit = PointOffSet(newHit);
        hit[1] = hit[0] = newHit;
        if (hit[0].transform.CompareTag("Wall"))
        {
            var point = hit[0].centroid - (dir * 0.01f);
            dir = Vector3.Reflect(dir, hit[0].normal);
            newHit = Physics2D.CircleCast(point, 0.1f, dir, _viewDis, layerMask: layerMask);
            newHit = PointOffSet(newHit);
            hit[1] = newHit;
        }
        _lineParticle.SetPosition(0, _shotPos);
        _lineParticle.SetPosition(1, hit[0].centroid);
        _lineParticle.SetPosition(2, hit[1].centroid);
        return;

        RaycastHit2D PointOffSet(RaycastHit2D hit)
        {
            hit.point= new Vector2(hit.point.x, hit.point.y - (HexagonGrid.I.CellSize.y / 2));
            return hit;
        }
    }

    public void SetVisualsActive(bool isActive)
    {
        _lineParticle.gameObject.SetActive(isActive);
        predictionBubble.gameObject.SetActive(isActive);   
    }


#if UNITY_EDITOR
    private readonly int _segments = 5;
    public bool showGizmos = true;
    public void OnDrawGizmos()
    {
        if (false == Application.isPlaying)
            return;
        if (false == showGizmos)
            return;
        Gizmos.color = Color.blue;

        Vector2 origin = _shotPos;
        var step = _viewAngle / _segments;

        // 각도 시작 = -viewAngle/2
        var prevPoint = origin + DirFromAngle(-_viewAngle / 2) * _viewDis;

        // 중심 → 양 끝 라인
        Gizmos.DrawLine(origin, prevPoint);
        Gizmos.DrawLine(origin, origin + DirFromAngle(_viewAngle / 2) * _viewDis);

        // 원호 그리기
        for (int i = 1; i <= _segments; ++i)
        {
            var curAngle = -_viewAngle / 2 + step * i;
            var nextPoint = origin + DirFromAngle(curAngle) * _viewDis;

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
