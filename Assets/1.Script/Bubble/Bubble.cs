using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bubble : SerializedMonoBehaviour
{
    [SerializeField] private Dictionary<BubbleType, Sprite> _typeSprites = new();
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [ShowInInspector] public BubbleType MyType { get; private set; }
    public static BubbleType GetRandomBubbleType => (BubbleType)Random.Range(1, 4);
    public Vector2Int Cell => HexagonGrid.I.GetPosToCellNumber(transform.position);
    public static Vector3 Scale => new(0.48f, 0.48f, 1f);
    private BubbleAttack _attackObj;
    [NonSerialized] public bool isAttack;
    
    public void SetType(BubbleType type)
    {
        MyType = type;
        _spriteRenderer.sprite = _typeSprites[type];
    }
    public void Drop(float dur, int score)
    {
        HexagonGrid.I.SetBubble(null, Cell, BubbleType.None);
        Utile.Move(transform,
            GameStepManager.I.holePos, 
            dur,
            () =>
            {
                var scoreObj = ObjectPoolManager.I.BubbleScorePool.Get();
                scoreObj.ShowScore(transform.position, score);
                ObjectPoolManager.I.BubblePool.Release(this);
                AttackObjRelease();
            });
    }
    public void Pop(float dur, int score)
    {
        if (MyType == BubbleType.None)
            return;

        var scoreObj = ObjectPoolManager.I.BubbleScorePool.Get();
        scoreObj.ShowScore(transform.position, score);

        HexagonGrid.I.SetBubble(null, Cell, BubbleType.None);
        ObjectPoolManager.I.BubblePool.Release(this);
        
        Attack();

        if (false == GameStepManager.I.energy.IsActive)
            return;
        var star = ObjectPoolManager.I.BubbleStarPool.Get();
        star.Set(MyType);
        star.transform.position = transform.position;
        Utile.Move(star.transform,
            Utile.RandomPointInCircle(GameStepManager.I.energy.gamePos, 0.3f),
            dur,
            () =>
            {
                GameStepManager.I.energy.AddEnergy(5, 2);
                ObjectPoolManager.I.BubbleStarPool.Release(star);
            });
    }

    private void Attack()
    {
        if (false == isAttack)
            return;

        _attackObj.transform.parent = null;
        var tr = GameStepManager.I.boss.transform;
        Utile.Move(
            _attackObj.transform,
            Utile.RandomPointInCircle(tr.position, 0.4f),
            2f,
            () =>
            {
                AttackObjRelease();
                GameStepManager.I.boss.GetDamage(7);
            },
            0.8f);
    }

    public void AttackObjRelease()
    {
        if (false == isAttack)
            return;
        ObjectPoolManager.I.BubbleAttackPool.Release(_attackObj);
        isAttack = false;
    }
    public void SetAttackBubble()
    {
        _attackObj = ObjectPoolManager.I.BubbleAttackPool.Get();
        _attackObj.transform.parent = transform;
        _attackObj.transform.localPosition = Vector3.zero;
        _attackObj.transform.localScale = Vector3.one;
        isAttack = true;
    }
}


public enum BubbleType
{
    None,
    Bule,
    Yellow,
    Red,
    Energy,
    Boom
}
