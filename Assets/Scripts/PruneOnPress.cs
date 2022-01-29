using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruneOnPress : MonoBehaviour
{
    [SerializeField] KeyCode keyPruneBlue;
    [SerializeField] KeyCode keyPruneRed;
    [SerializeField] KeyCode keyPruneGreen;
    [SerializeField] GameManager gameManager;
    
    private List<GameObject> orgsToPrune;

    [SerializeField] public float pruneCooldownPeriodSec = 3.0f;
    private float lastPruneSec;

    void Start()
    {
        lastPruneSec = 0;
    }


    // Update is called once per frame
    void Update()
    {
        float currentTimeSec = Time.time;
        bool isTimeToPrune = currentTimeSec - lastPruneSec > pruneCooldownPeriodSec;
        if (isTimeToPrune)
        {
            lastPruneSec = currentTimeSec;
            OrganismType pruneOrganismType = new OrganismType();
            //Identify plant type to prune
            if (Input.GetKey(keyPruneBlue))
            {
                pruneOrganismType = OrganismType.Blue;
            } else if (Input.GetKey(keyPruneGreen))
            {
                pruneOrganismType = OrganismType.Green;
            } else if (Input.GetKey(keyPruneRed))
            {
                pruneOrganismType = OrganismType.Red;
            }

            for (int rowIndex = 0; rowIndex < gameManager.numRows; rowIndex++)
            {
                for (int colIndex = 0; colIndex < gameManager.numCols; colIndex++)
                {
                    GameObject organism = gameManager.organismGrid[rowIndex][colIndex];
                    if (organism is null)
                    {
                        continue;
                    }

                }
            }

            

            //Prune %% of population
        }

    }
}
