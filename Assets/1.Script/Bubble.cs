using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Serial = UnityEngine.SerializeField;
public class Bubble : SerializedMonoBehaviour, IBubble
{
    [Serial] private Dictionary<BubbleType, Sprite> _typeSprites = new();
    [Serial] private BubbleType _myType;
    [Serial] private SpriteRenderer _spriteRenderer;
    
    public void SetType(BubbleType type)
    {
        _myType = type;
        _spriteRenderer.sprite = _typeSprites[type];
    }
    public void GetDamage(BubbleType type)
    {
    }
}


public enum BubbleType
{
    White,
    Yellow,
    Red,
    Green,
    Purple,
}
