using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow_shears : MonoBehaviour
{


    private void Start()
    {
        gameObject.SetActive(false);
    }



    void Update()
    {
        transform.position = Input.mousePosition + new Vector3(50f, 0f, 0f);
        if (Input.mousePosition.x <= Screen.width / 10)
        {

        }
    }
}
