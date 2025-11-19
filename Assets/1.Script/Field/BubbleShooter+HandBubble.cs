using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;using UnityEngine.EventSystems;

public partial class BubbleShooter : IPointerDownHandler
{
    private readonly Bubble[] _bubbles = new Bubble[3];
    private Vector3[] _threePos = new Vector3[3];
    private Vector3[] _threeAroundPos = new Vector3[3];
    private bool IsTwoBubble => _bubbles[2].MyType == BubbleType.None;
    public BubbleType CurrentBubbleType => _bubbles[0].MyType;
    public int bubbleCount = 22;


    private void InitBubbles()
    {
        _threePos = Utile.GetCirclePoints(transform.position, sr.size.y / 2, 3, 90).ToArray();
        _threeAroundPos = Utile.GetCirclePoints(transform.position, sr.size.y / 2, 3, 145).ToArray();

        for (int i = 0; i < _bubbles.Length; ++i)
        {
            _bubbles[i] = ObjectPoolManager.I.BubblePool.Get();
            _bubbles[i].gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            _bubbles[i].transform.localScale = new Vector3(0.35f, 0.35f, 1);
            _bubbles[i].transform.position = _threePos[i];
            _bubbles[i].SetType(Bubble.GetRandomBubbleType);
        }

        _bubbles[2].SetType(BubbleType.None);
    }
    public async void OnPointerDown(PointerEventData eventData)
    {
        
        await SwapBubble();
    }

    public void SetHandBubbleType(int index ,BubbleType type)
    {
        _bubbles[index].SetType(type);
    }
    private async Task SwapBubble()
    {
        if (false == activeControll)
            return;
        activeControll = false;
        if(IsTwoBubble)
        {
            _ = SwapBubbles(0, 1);
            await SwapBubbles(1, 0);
            (_bubbles[0], _bubbles[1]) = (_bubbles[1], _bubbles[0]);

        }
        else
        {
            _ = SwapBubbles(0, 2);
            _ = SwapBubbles(2, 1);
            await SwapBubbles(1, 0);
            (_bubbles[0], _bubbles[1], _bubbles[2]) = (_bubbles[2], _bubbles[1], _bubbles[0]);

        }
        activeControll = true;


    }

    public async Task RefillBubble()
    {
        activeControll = false;
        if (IsTwoBubble)
        {
            Scale(0);
            _ =  SwapBubbles(0, 1, 0);
            await SwapBubbles(1, 0);
            (_bubbles[0], _bubbles[1]) = (_bubbles[1], _bubbles[0]);
        }
        else
        {
            Scale(0);
            _ = SwapBubbles(0, 2, 0);
            _ = SwapBubbles(2, 1);
            await SwapBubbles(1, 0);
            (_bubbles[0], _bubbles[1], _bubbles[2]) = (_bubbles[2], _bubbles[1], _bubbles[0]);
        }
        activeControll = true;

        void Scale(int index)
        {
            _bubbles[index].SetType(Bubble.GetRandomBubbleType);
            _bubbles[index].transform.localScale = Vector3.zero;
            _bubbles[index].transform.DOScale(new Vector3(0.35f,0.35f,0.35f), 0.3f).SetEase(Ease.Linear);
        }
    }
    private async Task SwapBubbles(int targetIndex, int endIndex, float dur = 0.3f)
    {
        await RotateAroundPoint(
            _bubbles[targetIndex].transform, 
            _threePos[endIndex], 
            _threeAroundPos[targetIndex],
            dur);
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



}
