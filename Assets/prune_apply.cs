using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class prune_apply : MonoBehaviour, IPointerClickHandler
{
    public GameObject prune_shears;
    public bool apply_prune = false;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!apply_prune)
        {
            apply_prune = true;
            prune_shears.SetActive(true);
        }

        else
        {
            apply_prune = false;
        }



    }
}
