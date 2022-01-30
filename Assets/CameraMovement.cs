using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector3 cameraFocusPoint;
    public float cameraDistance, cameraHeight, rotationSpeed;

    private float rotationAngleDeg;

    // Start is called before the first frame update
    void Start()
    {
        rotationAngleDeg = 0.0f;  
    }

    // Update is called once per frame
    void Update()
    {
        rotationAngleDeg += rotationSpeed * Time.deltaTime;

        transform.position =
            cameraFocusPoint 
            - Quaternion.Euler(0, rotationAngleDeg, 0) * new Vector3(cameraDistance, 0, 0)
            + new Vector3(0, cameraHeight, 0);

        transform.LookAt(cameraFocusPoint);
    }
}
