using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class water_click : MonoBehaviour, IPointerClickHandler
{

    public bool apply_water= false;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!apply_water)
        {
            apply_water = true;
        }

        else
        {
            apply_water = false;
        }
        
        

    }
}
