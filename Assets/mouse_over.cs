using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class mouse_over : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool should_rotate = false;
    public void OnPointerEnter(PointerEventData eventData)
    {
        should_rotate = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        should_rotate = false;
    }
}
