using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class BubbleScore : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    public void ShowScore(Vector3 pos, int score)
    {
        text.text = score.ToString();
        ScoreHelper.TotalScore += score;
        transform.position = pos;
        transform.DOMove(transform.position + new Vector3(0, 0.4f, 0), .4f).OnComplete(() =>
        {
            ObjectPoolManager.I.BubbleScorePool.Release(this);
        });
    }
}
