using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class apply_creeper : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool apply2creeper;
    public GameObject shears;

    public void OnPointerDown(PointerEventData eventData)
    {
        apply2creeper = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        apply2creeper = false;
    shears.SetActive(false);
    }
}
