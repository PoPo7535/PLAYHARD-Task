using System;
using System.Numerics;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Serialization;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class BubbleShooter : MonoBehaviour
{
    public float viewDis = 10f;
    public float viewAngle = 90f;
    private int segments = 5;         // 라인 분해도

    public GameObject[] test;
    
    [Button]
    public RaycastHit2D[] Shooter()
    {
        var result = new RaycastHit2D[2];

        var screenPos = GetPointerWorldPosition();
        var dir = (screenPos - (Vector2)transform.position).normalized;
        
        // 각도확인
        if (Vector3.Angle(transform.forward, dir) < viewAngle / 2f)
            return null;

        result[0] = Physics2D.Raycast(transform.position, dir, viewDis);
        if (result[0].transform.CompareTag("Wall"))
        {
            var point = result[0].point - (dir * 0.01f);
            dir = Vector3.Reflect(dir, result[0].normal);
            result[1] = Physics2D.Raycast(point, dir, viewDis);
        }
        test[0].transform.position = result[0].point;
        test[1].transform.position = result[1].point;
        return result;
    }

    private Vector2 GetPointerWorldPosition()
    {
        Vector2 screenPos;

        // PC 마우스
        if (Input.touchCount == 0)
            screenPos = Input.mousePosition;
        // 모바일 터치
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
        var step = viewAngle / segments;

        // 각도 시작 = -viewAngle/2
        var prevPoint = origin + DirFromAngle(-viewAngle / 2) * viewDis;

        // 중심 → 양 끝 라인
        Gizmos.DrawLine(origin, prevPoint);
        Gizmos.DrawLine(origin, origin + DirFromAngle(viewAngle / 2) * viewDis);

        // 원호 그리기
        for (int i = 1; i <= segments; ++i)
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
