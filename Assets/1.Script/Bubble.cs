using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using Serial = UnityEngine.SerializeField;
public class Bubble : SerializedMonoBehaviour, IBubble
{
    [Serial] private Dictionary<BubbleType, Sprite> _typeSprites = new();
    [Serial] private SpriteRenderer _spriteRenderer;
    public BubbleType MyType { get; private set; }
    
    public void SetType(BubbleType type)
    {
        MyType = type;
        _spriteRenderer.sprite = _typeSprites[type];
    }

    public void Drop()
    {
        
    }
    public void GetDamage(BubbleType type)
    {
    }

}


public enum BubbleType
{
    None,
    Bule,
    Yellow,
    Red,
    Green,
    Purple,
}
