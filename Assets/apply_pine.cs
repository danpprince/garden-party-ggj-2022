using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class apply_pine : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool apply2pine;
    public GameObject shears;

    public void OnPointerDown(PointerEventData eventData)
    {
        apply2pine = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        apply2pine = false;
        shears.SetActive(false);
    }
}



