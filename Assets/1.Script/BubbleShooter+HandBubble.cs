using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class BubbleShooter 
{
    private readonly Bubble[] _bubbles = new Bubble[3];
    private Vector3[] _twoPos = new Vector3[2];
    private Vector3[] _twoAroundPos = new Vector3[2];
    private Vector3[] _threePos = new Vector3[3];
    private Vector3[] _threeAroundPos = new Vector3[3];
    private bool IsTwoBubble => _bubbles[2].MyType == BubbleType.None;
    private void InitBubbles()
    {
        _twoPos = Utile.GetCirclePoints(transform.position, sr.size.y / 2, 2, 90).ToArray();
        _twoAroundPos = Utile.GetCirclePoints(transform.position, sr.size.y / 2, 2, 180).ToArray();
        _threePos = Utile.GetCirclePoints(transform.position, sr.size.y / 2, 3, 90).ToArray();
        _threeAroundPos = Utile.GetCirclePoints(transform.position, sr.size.y / 2, 3, 135).ToArray();

        for (int i = 0; i < _bubbles.Length; ++i)
        {
            _bubbles[i] = ObjectPoolManager.I.BubblePool.Get();
            _bubbles[i].transform.localScale = new Vector3(0.35f, 0.35f, 1);
            _bubbles[i].transform.position = _threeAroundPos[i];
            _bubbles[i].SetType(Bubble.GetRandomBubbleType);
        }

        _bubbles[2].SetType(BubbleType.None);
    }
    
    [Button]
    public async Task SwapBubble()
    {
        if(IsTwoBubble)
        {
            _ = SwapBubbles(0, 1);
            await SwapBubbles(1, 0);
            (_bubbles[0], _bubbles[1]) = (_bubbles[1], _bubbles[0]);

        }
        else
        {
            _ = SwapBubbles(0, 1);
            _ = SwapBubbles(1, 2);
            await SwapBubbles(2, 0);
            (_bubbles[0], _bubbles[1], _bubbles[2]) = (_bubbles[2], _bubbles[1], _bubbles[0]);

        }

        async Task SwapBubbles(int targetIndex, int endIndex)
        {
            var targetPos = IsTwoBubble ? _twoPos[endIndex] : _threePos[endIndex];
            var targetAroundPos = IsTwoBubble ? _twoAroundPos[targetIndex] : _threeAroundPos[targetIndex];
            await RotateAroundPoint(_bubbles[targetIndex].transform, targetPos,targetAroundPos);
        }
    }

    public void RefillBubble()
    {
        if (_bubbles[0].MyType == BubbleType.None)
        {
            _bubbles[0].SetType(Bubble.GetRandomBubbleType);
            return;
        }
        if (_bubbles[2].MyType == BubbleType.None)
        {
            _bubbles[2].SetType(Bubble.GetRandomBubbleType);
            return;
        }
    }

    private async Task RotateAroundPoint(Transform target, Vector3 targetPosition, Vector3 controlPoint, float dur = 0.3f)
    {
        // 경로 설정: 현재 → 곡선 → 목표
        var path = new Vector3[]
        {
            target.position,
            controlPoint,
            targetPosition
        };

        var isComplete = false;
        // 이동 (회전 궤적처럼 곡선 경로로)
        target.DOPath(path, dur, PathType.CatmullRom)
            .SetEase(Ease.InOutCubic).OnComplete(() => isComplete = true);
        await new WaitUntil(() => isComplete);
    }

    public BubbleType CurrentBubble => BubbleType.Bule;

}
