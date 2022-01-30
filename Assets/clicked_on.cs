using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class clicked_on : MonoBehaviour, IPointerDownHandler, IPointerUpHandler  
{
    public bool apply_butterfly;
    public GameObject shears;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        apply_butterfly = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        apply_butterfly = false;
        shears.SetActive(false);
    }
}



