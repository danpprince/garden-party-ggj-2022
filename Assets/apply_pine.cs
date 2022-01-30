using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class apply_pine : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool apply2pine;

    public void OnPointerDown(PointerEventData eventData)
    {
        apply2pine = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        apply2pine = false;
    }
}
