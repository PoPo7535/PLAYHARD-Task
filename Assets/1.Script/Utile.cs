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
}
