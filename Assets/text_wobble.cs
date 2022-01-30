using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
public class text_wobble : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    TMP_Text textMesh;
    Mesh mesh;
    Vector3[] vertices;
    private bool should_wobble = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        should_wobble = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        should_wobble = false;
    }

    void Start()
    {
        textMesh = GetComponent<TMP_Text>();
        
    }

    // Update is called once per frame
    void Update()
    {

        

        
            textMesh.ForceMeshUpdate();
            mesh = textMesh.mesh;
            vertices = mesh.vertices;

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 offset = Wobble(Time.time + i);

                vertices[i] = vertices[i] + offset;
            }

            if (should_wobble) { 
            mesh.vertices = vertices;
            textMesh.canvasRenderer.SetMesh(mesh);
        }
    }
Vector2 Wobble(float time)
    {
        return new Vector2(Mathf.Sin(time * 3.3f), Mathf.Cos(time * 1.8f));
    }
}


