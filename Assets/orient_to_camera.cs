using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orient_to_camera : MonoBehaviour
{

    public GameObject camera;
    private float x_camera;
    private float y_camera;
    private float z_camera;
    private float size_camera;
    public GameObject UI_object;
    private bool should_rotate;
    private float spin;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        x_camera = camera.GetComponent<Transform>().position.x;
        y_camera = camera.GetComponent<Transform>().position.y;
        z_camera = camera.GetComponent<Transform>().position.z;
        size_camera = camera.GetComponent<Camera>().orthographicSize;
        
        

        gameObject.GetComponent<Transform>().position = new Vector3(gameObject.GetComponent<Transform>().position.x, y_camera - (size_camera/2) , z_camera);



    }

    
    void Update()
    {
        should_rotate = UI_object.GetComponent<mouse_over>().should_rotate;

        if (should_rotate)
        {
            
            transform.Rotate(0, 1.25f, 0, Space.World);
        }
    }
}
