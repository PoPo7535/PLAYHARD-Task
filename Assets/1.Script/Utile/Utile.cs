using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

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
        var screen = RectTransformUtility.WorldToScreenPoint(null, ui.position);

        var z = Mathf.Abs(Camera.main.transform.position.z);
        return Camera.main.ScreenToWorldPoint(new Vector3(screen.x, screen.y, z));
    }
    public static Vector2 RandomPointInCircle(Vector2 center, float radius)
    {
        var t = Random.value;              
        var r = radius * Mathf.Sqrt(t);    
        var angle = Random.Range(0f, Mathf.PI * 2f);

        var x = center.x + r * Mathf.Cos(angle);
        var y = center.y + r * Mathf.Sin(angle);

        return new Vector2(x, y);
    }
    public static void Move(Transform tr, Vector3 endPos, float totalDuration, Action completeActive, float scale = 0.2f)
    {
        var startPos = tr.position;
        
        var randomDir = Random.insideUnitCircle.normalized;
        var scatterDistance = Random.Range(0.5f, 1.5f);
        var scatterTarget = startPos + (Vector3)(randomDir * scatterDistance);
        
        var scatterDuration = totalDuration * 0.15f;
        var suckDuration = totalDuration * 0.85f;

        var shrinkDuration = suckDuration * 0.5f;
        var shrinkDelay = suckDuration - shrinkDuration;

        var controlPoint = (scatterTarget + new Vector3(0, -5f, 0)) * 0.5f
                           + new Vector3(Random.Range(-2f, 2f), Random.Range(1.5f, 2.5f), 0f);

        var seq = DOTween.Sequence();

        seq.Append(tr.DOMove(scatterTarget, scatterDuration).SetEase(Ease.OutQuad));

        seq.Append(tr.DOPath(
                new[] { scatterTarget, controlPoint, endPos },
                suckDuration,
                PathType.CatmullRom)
            .SetEase(Ease.InOutCubic));

        seq.Join(tr.DORotate(new Vector3(0, 0, 720f), suckDuration, RotateMode.FastBeyond360));
        seq.Join(tr.DOScale(new Vector3(scale, scale, scale), shrinkDuration).SetDelay(shrinkDelay));
        seq.OnComplete(() => completeActive?.Invoke());
    }
    public static Vector2 UIToWorldSize(RectTransform rect)
    {
        var pixelSize = rect.rect.size;
        var worldPerPixel = (Camera.main.orthographicSize * 2f) / Screen.height;
        return pixelSize * worldPerPixel;
    }
}
