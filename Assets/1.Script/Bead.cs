using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Serial = UnityEngine.SerializeField;
public class Bead : SerializedMonoBehaviour, IBead
{
    [Serial] private Dictionary<BeadType, Sprite> _typeSprites = new();
    [Serial] private BeadType _myType;
    [Serial] private SpriteRenderer _spriteRenderer;
    
    public void SetType(BeadType type)
    {
        _myType = type;
        _spriteRenderer.sprite = _typeSprites[type];
    }
    public void GetDamage(BeadType type)
    {
    }
}


public enum BeadType
{
    White,
    Yellow,
    Red,
    Green,
    Purple,
}
