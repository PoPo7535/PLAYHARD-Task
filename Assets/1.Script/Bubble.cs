using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using Serial = UnityEngine.SerializeField;
public class Bubble : SerializedMonoBehaviour, IBubble
{
    [Serial] private Dictionary<BubbleType, Sprite> _typeSprites = new();
    [Serial] private SpriteRenderer _spriteRenderer;
    public Vector2Int Cell => HexagonGrid.I.GetPosToCellNumber(transform.position);
    public static Vector3 Scale => new(0.48f, 0.48f, 1f);
    [ShowInInspector] public BubbleType MyType { get; private set; }
    
    
    public void SetType(BubbleType type)
    {
        MyType = type;
        _spriteRenderer.sprite = _typeSprites[type];
    }


    public void Drop(float totalDuration = 1f, float off = 0.1f)
    {
        HexagonGrid.I.SetBubble(null, Cell, BubbleType.None);
        var startPos = transform.position;
        totalDuration += off;
        var randomDir = Random.insideUnitCircle.normalized;
        var scatterDistance = Random.Range(0.5f, 1.5f);
        var scatterTarget = startPos + (Vector3)(randomDir * scatterDistance);
        
        var scatterDuration = totalDuration * 0.15f;
        var suckDuration = totalDuration * 0.85f;

        var shrinkDuration = suckDuration * 0.5f;
        var shrinkDelay = suckDuration - shrinkDuration;

        var endPos = new Vector3(0, -3f, 0);
        var controlPoint = (scatterTarget + new Vector3(0, -5f, 0)) * 0.5f
                           + new Vector3(Random.Range(-2f, 2f), Random.Range(1.5f, 2.5f), 0f);

        var seq = DOTween.Sequence();

        seq.Append(transform.DOMove(scatterTarget, scatterDuration).SetEase(Ease.OutQuad));

        seq.Append(transform.DOPath(
                new[] { scatterTarget, controlPoint, endPos },
                suckDuration,
                PathType.CatmullRom)
            .SetEase(Ease.InOutCubic));

        seq.Join(transform.DORotate(new Vector3(0, 0, 720f), suckDuration, RotateMode.FastBeyond360));

        seq.Join(transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), shrinkDuration).SetDelay(shrinkDelay));

        seq.OnComplete(() => ObjectPoolManager.I.BubblePool.Release(this));
        seq.Duration();
    }
    public void GetDamage(BubbleType type)
    {
    }

    public static BubbleType GetRandomBubbleType => (BubbleType)Random.Range(1, 4);

}


public enum BubbleType
{
    None,
    Bule,
    Yellow,
    Red,
    Energy
}
