using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bubble : SerializedMonoBehaviour
{
    [SerializeField] private Dictionary<BubbleType, Sprite> _typeSprites = new();
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public static BubbleType GetRandomBubbleType => (BubbleType)Random.Range(1, 4);
    public Vector2Int Cell => HexagonGrid.I.GetPosToCellNumber(transform.position);
    public static Vector3 Scale => new(0.48f, 0.48f, 1f);
    [ShowInInspector] public BubbleType MyType { get; private set; }
    
    public void SetType(BubbleType type)
    {
        MyType = type;
        _spriteRenderer.sprite = _typeSprites[type];
    }

    public void Drop(float totalDuration = 1f)
    {
        HexagonGrid.I.SetBubble(null, Cell, BubbleType.None);

        Move(transform, 
            new Vector3(0, -3, 0), 
            totalDuration, 
            () => { ObjectPoolManager.I.BubblePool.Release(this); });
        // var startPos = transform.position;
        //
        // var randomDir = Random.insideUnitCircle.normalized;
        // var scatterDistance = Random.Range(0.5f, 1.5f);
        // var scatterTarget = startPos + (Vector3)(randomDir * scatterDistance);
        //
        // var scatterDuration = totalDuration * 0.15f;
        // var suckDuration = totalDuration * 0.85f;
        //
        // var shrinkDuration = suckDuration * 0.5f;
        // var shrinkDelay = suckDuration - shrinkDuration;
        //
        // var endPos = new Vector3(0, -3f, 0);
        // var controlPoint = (scatterTarget + new Vector3(0, -5f, 0)) * 0.5f
        //                    + new Vector3(Random.Range(-2f, 2f), Random.Range(1.5f, 2.5f), 0f);
        //
        // var seq = DOTween.Sequence();
        //
        // seq.Append(transform.DOMove(scatterTarget, scatterDuration).SetEase(Ease.OutQuad));
        //
        // seq.Append(transform.DOPath(
        //         new[] { scatterTarget, controlPoint, endPos },
        //         suckDuration,
        //         PathType.CatmullRom)
        //     .SetEase(Ease.InOutCubic));
        //
        // seq.Join(transform.DORotate(new Vector3(0, 0, 720f), suckDuration, RotateMode.FastBeyond360));
        // seq.Join(transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), shrinkDuration).SetDelay(shrinkDelay));
        // seq.OnComplete(() => ObjectPoolManager.I.BubblePool.Release(this));
    }
    public void PoP(float dur = 1f)
    {
        var star = ObjectPoolManager.I.BubbleStarPool.Get();
        star.Set(MyType);
        star.transform.position = transform.position;
        Move(star.transform, GameStepManager.I.energy.gamePos, dur, 
            () => { ObjectPoolManager.I.BubbleStarPool.Release(star); } );
        HexagonGrid.I.SetBubble(null, Cell, BubbleType.None);
        ObjectPoolManager.I.BubblePool.Release(this);
    }
    // var endPos = new Vector3(0, -3f, 0);

    private void Move(Transform tr, Vector3 endPos, float totalDuration, Action completeActive)
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
        seq.Join(tr.DOScale(new Vector3(0.2f, 0.2f, 0.2f), shrinkDuration).SetDelay(shrinkDelay));
        seq.OnComplete(() => completeActive?.Invoke());
    }
}


public enum BubbleType
{
    None,
    Bule,
    Yellow,
    Red,
    Energy
}
