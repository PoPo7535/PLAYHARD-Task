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
    public BubbleType MyType { get; private set; }
    
    public void SetType(BubbleType type)
    {
        MyType = type;
        _spriteRenderer.sprite = _typeSprites[type];
    }

    public void Drop()
    {
        // Debug.Log($"Drop{HexagonGrid.I.GetPosToCellNumber(transform.position)}");
        var cell = HexagonGrid.I.GetPosToCellNumber(transform.position);
        HexagonGrid.I.SetBubble(null, cell, BubbleType.None);
        var randomDir = Random.insideUnitCircle.normalized;
        var scatterDistance = Random.Range(0.5f, 1.5f);
        var scatterTarget = transform.position + (Vector3)(randomDir * scatterDistance);

        var seq = DOTween.Sequence();

        // 1단계: 퍼짐
        seq.Append(transform.DOMove(scatterTarget, 0.3f).SetEase(Ease.OutQuad));

        // 2단계: 블랙홀 방향 이동 + 크기 줄이기
        seq.Append(transform.DOMove(new Vector3(0, -3, 0), 0.7f).SetEase(Ease.InQuad));
        seq.Join(transform.DOScale(Vector3.zero, 0.7f));

        // (선택) 회전도 추가
        seq.Join(transform.DORotate(new Vector3(0, 0, 360), 0.7f, RotateMode.FastBeyond360));

        // 3단계: 제거
        seq.OnComplete(() => BubblePool.I.Pool.Release(this));
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
