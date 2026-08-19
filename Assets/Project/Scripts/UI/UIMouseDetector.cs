using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMouseDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool _hoverd = false;
    public bool Hoverd => _hoverd;

    public Action onEnter;
    public Action onExit;

    public void OnPointerEnter(PointerEventData eventData) {
        _hoverd = true;
        onEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData) {
        _hoverd = false;
        onExit?.Invoke();
    }
}
