using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;using UnityEngine.EventSystems;

public partial class BubbleShooter : IPointerDownHandler
{
    private readonly Bubble[] _bubbles = new Bubble[3];
    private Vector3[] _threePos = new Vector3[3];
    private Vector3[] _threeAroundPos = new Vector3[3];
    [SerializeField] private TMP_Text _bubbleCountText;
    [SerializeField] private int _bubbleCount = 22;
    private bool IsTwoBubble => _bubbles[2].MyType == BubbleType.None;
    public BubbleType CurrentBubbleType => _bubbles[0].MyType;

    private void InitBubbles()
    {
        _threePos = Utile.GetCirclePoints(transform.position, _sr.size.y / 2, 3, 90).ToArray();
        _threeAroundPos = Utile.GetCirclePoints(transform.position, _sr.size.y / 2, 3, 145).ToArray();

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

    public async Task SpareBubbleToScore()
    {
        foreach (var bubble in _bubbles)
        {
            bubble.Pop(50);
        }
        for (int i = 0; i < _bubbleCount; ++i)
        {
            await UniTask.Delay(300);
            var bubble = ObjectPoolManager.I.BubblePool.Get();
            bubble.SetType(Bubble.GetRandomBubbleType);
            bubble.transform.position = transform.position;
            var pos = Utile.RandomPointInCircle(new Vector3(0, 0, 0), 1);
            var newNumber = i + 1;
            bubble.transform.DOMove(pos, 1).OnComplete(() =>
            {
                bubble.Pop(1000 * (newNumber));
            });
        }
    }

    public async void OnPointerDown(PointerEventData eventData)
    {
        await SwapBubble();
    }

    public void SetHandBubbleType(int index ,BubbleType type)
    {
        _bubbles[index].SetType(type);
    }
    public void SetEnergyBubble(Bubble bubble, int setEnergyIndex)
    {
        activeControll = false;
        bubble.SetType(BubbleType.Energy);
        bubble.transform
            .DOMove(_threePos[setEnergyIndex], 0.3f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                ObjectPoolManager.I.BubblePool.Release(bubble);
                activeControll = true;
                _bubbles[setEnergyIndex].SetType(BubbleType.Energy);
            });
    }
    public async Task SendEnergy()
    {
        activeControll = false;
        var isComplete = false;
        var bubble = ObjectPoolManager.I.BubblePool.Get();
        bubble.SetType(_bubbles[0].MyType);
        _bubbles[0].SetType(BubbleType.None);
        bubble.transform.position = _bubbles[0].transform.position;
        bubble.transform.localScale = new Vector3(0.35f, 0.35f, 1);
        bubble.transform.DOMove(GameStepManager.I.energy.gamePos, 0.3f).OnComplete(() =>
        {
            ObjectPoolManager.I.BubblePool.Release(bubble);
            isComplete = true;
        });
        await new WaitUntil(() => isComplete);
        var fullCharge = GameStepManager.I.energy.AddEnergy(25, 0);
        if (false == fullCharge)
            await RefillBubble();
        activeControll = true;
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
            (_bubbles[0], _bubbles[1], _bubbles[2]) = (_bubbles[1], _bubbles[2], _bubbles[0]);
        } 
        activeControll = true;
    }

    public bool NoBubbles()
    {
        if (0 == _bubbleCount &&
            _bubbles[0].MyType == BubbleType.None &&
            _bubbles[1].MyType == BubbleType.None &&
            _bubbles[2].MyType == BubbleType.None)
            return true;
        return false;
    }
    public async Task RefillBubble()
    {
        _bubbleCountText.text = _bubbleCount.ToString();
        if (_bubbleCount == 0)
            return;
        --_bubbleCount;
        
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
            (_bubbles[0], _bubbles[1], _bubbles[2]) = (_bubbles[1], _bubbles[2], _bubbles[0]);
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
        var path = new[]
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
