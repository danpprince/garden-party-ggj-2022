using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class mouse_over : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AudioSource mouseOver;

    public bool should_rotate = false;
    public void OnPointerEnter(PointerEventData eventData)
    {
        should_rotate = true;
        mouseOver.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        should_rotate = false;
    }
}
