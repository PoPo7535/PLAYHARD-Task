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
    
    public static List<Vector3> GetCirclePoints(Vector3 center, float radius, int count, float angleOffsetDegrees)
    {
        var points = new List<Vector3>();
        var angleOffset = angleOffsetDegrees * Mathf.Deg2Rad;

        for (int i = 0; i < count; ++i)
        {
            var angle = -2 * Mathf.PI * i / count + angleOffset;  
            var x = center.x + radius * Mathf.Cos(angle);
            var y = center.y + radius * Mathf.Sin(angle);
            points.Add(new Vector3(x, y, center.z));
        }
        return points;
    }
    public static Vector3 UIToWorld(RectTransform ui)
    {
        Vector2 screen = RectTransformUtility.WorldToScreenPoint(null, ui.position);

        float z = Mathf.Abs(Camera.main.transform.position.z);
        return Camera.main.ScreenToWorldPoint(new Vector3(screen.x, screen.y, z));
    }
    public static Vector2 UIToWorldSize(RectTransform rect)
    {
        var pixelSize = rect.rect.size;
        var worldPerPixel = (Camera.main.orthographicSize * 2f) / Screen.height;
        return pixelSize * worldPerPixel;
    }
}
