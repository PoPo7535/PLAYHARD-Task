using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BubbleEnergy : MonoBehaviour
{
    [SerializeField] private BubbleShooter _shooter;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Button _button;
    [SerializeField] private Image _energyImg;
    [SerializeField] private Image _fillImg;
    private Vector3 _gamePos;
    private float _energy = 0f;

    public void Start()
    {
        _gamePos = Utile.UIToWorld(_rectTransform);
        _button.onClick.AddListener(OnClick);
    }
    
    public void OnClick()
    {
        if (false == _shooter.activeControll)
            return;
        // if (_energy < 100f)
        //     return;
        _energy = 0;
        ActiveEnergyImg(false);
        var bubble = ObjectPoolManager.I.BubblePool.Get();
        bubble.transform.position = _gamePos;
        _ = _shooter.SetEnergyBubble(bubble);
    }

    private void ActiveEnergyImg(bool active)
    {
        _fillImg.fillAmount = 1;
        _energyImg.rectTransform.DOScale(active ? Vector3.one : Vector3.zero, 0.3f);
    }
    public void Foo()
    {
        Debug.Log(Utile.UIToWorld(_rectTransform));
    }
    public void AddEnergy(float energy)
    {
    }
}
