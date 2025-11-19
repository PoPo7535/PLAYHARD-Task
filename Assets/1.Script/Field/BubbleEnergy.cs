using UnityEngine;
using UnityEngine.EventSystems;

public class BubbleEnergy : MonoBehaviour, IPointerDownHandler
{
    private BubbleShooter _shooter;
    private float _energy = 0f;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (false == _shooter.activeControll)
            return;
        if (_energy < 100f)
            return;
        _energy = 0;
    }
}
