using System;
using UnityEngine;

public class BubbleStar : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sr;


    public void Set(BubbleType bubble)
    {
        _sr.color = GetColor(bubble);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    private Color GetColor(BubbleType bubble)
    {
        return bubble switch
        {
            BubbleType.None => Color.magenta,
            BubbleType.Bule => Color.blue,
            BubbleType.Yellow => Color.yellow,
            BubbleType.Red => Color.red,
            BubbleType.Energy => Color.white,
            _ => Color.magenta
        };
    }
}
