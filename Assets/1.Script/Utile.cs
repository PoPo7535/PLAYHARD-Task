using System.Collections.Generic;
using UnityEngine;

public static class Utile
{
    public static Vector2 GetPointerWorldPosition()
    {
        Vector2 screenPos;

        if (Input.touchCount == 0)
            screenPos = Input.mousePosition;
        else
            screenPos = Input.GetTouch(0).position;

        return Camera.main.ScreenToWorldPoint(screenPos);
    }
    
    public static List<Vector3> GetCirclePoints(Vector3 center, float radius, int count)
    {
        var points = new List<Vector3>();

        for (int i = 0; i < count; i++)
        {
            float angle = 2 * Mathf.PI * i / count; // 각도 (라디안)
            float x = center.x + radius * Mathf.Cos(angle);
            float y = center.y + radius * Mathf.Sin(angle); // 2D 기준 (XY 평면)
            points.Add(new Vector3(x, y, center.z)); // Z값은 그대로 유지
        }
        return points;
    }
}
