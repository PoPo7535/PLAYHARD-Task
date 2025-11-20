using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utility;

public class PopUp : LocalSingleton<PopUp>
{
    [SerializeField] private CanvasGroup _cg;
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _btnText;

    public void ShowPopUp(string msg, string btnMsg, Action onClick)
    {
        Active(true);
        _text.text = msg;
        _btnText.text = btnMsg;
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => onClick?.Invoke());
    }

    public void ClosePopUp()
    {
        Active(false);
    }

    private void Active(bool isActive)
    {
        _cg.alpha = isActive ? 1 : 0;
        _cg.interactable = isActive;
        _cg.blocksRaycasts = isActive;
    }
}
