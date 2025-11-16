using System;
using System.Numerics;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class BubbleShooter : MonoBehaviour
{
    public LineParticle lineParticle;
    public HexagonGrid grid;
    public float viewDis = 10f;
    public float viewAngle = 90f;
    private int _segments = 5;  

    private Bubble _predictionBubble;

    public void Start()
    {
        _predictionBubble = BubblePool.I.Pool.Get();
        _predictionBubble.tag = "Untagged";
        _predictionBubble.gameObject.SetActive(false);
        _predictionBubble.SetType(BubbleType.Bule);
        _predictionBubble.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 50);
        _predictionBubble.GetComponent<CircleCollider2D>().enabled = false;
    }

    public void Update()
    {
        if (Input.GetMouseButtonUp(0) ||
            Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            lineParticle.gameObject.SetActive(false);
            _predictionBubble.gameObject.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0) ||
            Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            lineParticle.gameObject.SetActive(true);
            _predictionBubble.gameObject.SetActive(true);
        }
        
        if (Input.GetMouseButton(0) ||
            Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            var hit = Shooter();
            if (hit.IsUnityNull())
                return;
            if (hit[1].transform.CompareTag("Bubble"))
            {
                _predictionBubble.transform.position = grid.GetGridPosition(grid.GetHitPointToCell(hit[1]));
            }
        }
    }

    private RaycastHit2D[] Shooter()
    {
        var result = new RaycastHit2D[2];

        var screenPos = GetPointerWorldPosition();
        var dir = (screenPos - (Vector2)transform.position).normalized;
        if (viewAngle / 2f < Vector2.Angle(transform.up, dir))
            return null;

        result[1] = result[0] = Physics2D.CircleCast(transform.position, 0.07f, dir, viewDis);
        if (result[0].transform.CompareTag("Wall"))
        {
            var point = result[0].point - (dir * 0.01f);
            dir = Vector3.Reflect(dir, result[0].normal);
            result[1] = Physics2D.Raycast(point, dir, viewDis);
        }
        lineParticle.SetPosition(0, transform.position);
        lineParticle.SetPosition(1, result[0].point);
        lineParticle.SetPosition(2, result[1].point);
        return result;
    }

    private Vector2 GetPointerWorldPosition()
    {
        Vector2 screenPos;

        if (Input.touchCount == 0)
            screenPos = Input.mousePosition;
        else
            screenPos = Input.GetTouch(0).position;

        return Camera.main.ScreenToWorldPoint(screenPos);
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
