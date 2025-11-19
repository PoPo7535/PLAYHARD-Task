using System;
using UnityEngine;

public class BubbleStar : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sr;


    public void Set(BubbleType bubble)
    {
        _sr.color = GetColor(bubble);
    }
    private Color GetColor(BubbleType bubble)
    {
        return bubble switch
        {
            BubbleType.None => Color.black,
            BubbleType.Bule => Color.blue,
            BubbleType.Yellow => Color.yellow,
            BubbleType.Red => Color.red,
            BubbleType.Energy => Color.black,
            _ => Color.black
        };
    }
}
