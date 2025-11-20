using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BubbleEnergy : MonoBehaviour
{
    private BubbleShooter shooter => GameStepManager.I.shooter;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Button _button;
    [SerializeField] private Image _energyImg;
    [SerializeField] private Image _fillImg;
    [NonSerialized] public Vector3 gamePos;
    private bool IsFullEnergy => 100f <= Energy;

    public bool IsActive { get; private set; } = true;
    private float _energy;
    private float Energy
    {
        get => _energy;
        set
        {
            _fillImg.fillAmount = 1 - (value / 100);
            _energy = value;
        }
    }

    public void Start()
    {
        gamePos = Utile.UIToWorld(_rectTransform);
        _button.onClick.AddListener(OnClick);
    }
    
    public void OnClick()
    {
        if (false == IsActive)
            return;
        if (false == shooter.activeControll)
            return;
        if (IsFullEnergy)
            return;
        _ = shooter.SendEnergy();
    }

    private void SetEnergyBubble(int setEnergyIndex)
    {
        SetActive(false);
        var bubble = ObjectPoolManager.I.BubblePool.Get();
        bubble.transform.position = gamePos;
        shooter.SetEnergyBubble(bubble, setEnergyIndex);
    }

    public bool AddEnergy(int addEnergy, int setEnergyIndex)
    {
        if (false == IsActive)
            return false;
        Energy += addEnergy;
        if (IsFullEnergy)
        {
            SetEnergyBubble(setEnergyIndex);
            Energy = 0;
            return true;
        }
        return false;
    }
    public void SetActive(bool active)
    {
        _energyImg.rectTransform.DOScale(active ? Vector3.one : Vector3.zero, 0.3f);
        IsActive = active;
    }
}
