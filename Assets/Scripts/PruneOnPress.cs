using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruneOnPress : MonoBehaviour
{
    [SerializeField] KeyCode keyPruneBlue;
    [SerializeField] KeyCode keyPruneRed;
    [SerializeField] KeyCode keyPruneGreen;
    [SerializeField] public float pruneCooldownPeriodSec = 3.0f;
    private float lastPruneSec;
    private List<GameObject> orgsToPrune;

    void Start()
    {
        lastPruneSec = 0;
    }

    // Update is called once per frame
    void Update()
    {
        bool isTimeToPrune = Time.time - lastPruneSec > pruneCooldownPeriodSec;
        if (isTimeToPrune)
        {
            //Identify plant type to prune
            if (Input.GetKey(keyPruneBlue))
            {
                
            } else if (Input.GetKey(keyPruneGreen))
            {
                
            } else if (Input.GetKey(keyPruneRed))
            {
                
            }

            //Prune %% of population
        }

    }
}
