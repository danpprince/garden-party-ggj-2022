using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruneOnPress : MonoBehaviour
{
    [SerializeField] KeyCode keyPruneBlue;
    [SerializeField] KeyCode keyPruneRed;
    [SerializeField] KeyCode keyPruneGreen;
    [SerializeField] GameManager gameManager;
    [SerializeField] float popPercentToPrune;
    [SerializeField] public float pruneCooldownPeriodSec = 1.0f;
    private float lastPruneSec;
    private bool isPruneTypeSelected = false;

    void Start()
    {
        lastPruneSec = 0;
    }

    // Update is called once per frame
    void Update()
    {
        OrganismType pruneOrganismType = new OrganismType();
        //Identify plant type to prune
        if (Input.GetKey(keyPruneBlue))
        {
            pruneOrganismType = OrganismType.Blue;
            isPruneTypeSelected = true;
            Debug.Log("Prune Blue Trees");
        } else if (Input.GetKey(keyPruneGreen))
        {
            pruneOrganismType = OrganismType.Green;
            isPruneTypeSelected = true;
            Debug.Log("Prune Green Vines");
        } else if (Input.GetKey(keyPruneRed))
        {
            pruneOrganismType = OrganismType.Red;
            isPruneTypeSelected = true;
            Debug.Log("Prune Red Weeds");
        }
        float currentTimeSec = Time.time;
        bool isTimeToPrune = currentTimeSec - lastPruneSec > pruneCooldownPeriodSec;
        if (isTimeToPrune && isPruneTypeSelected)
        {            
            isPruneTypeSelected = false;
            lastPruneSec = currentTimeSec;     
            //crawl through grid looking for organisms, check the type for pruning
            for (int rowIndex = 0; rowIndex < gameManager.numRows; rowIndex++)
            {
                for (int colIndex = 0; colIndex < gameManager.numCols; colIndex++)
                {
                    GameObject organism = gameManager.organismGrid[rowIndex][colIndex];
                    if (organism is null)
                    {
                        continue;
                    }
                    if (organism.GetComponent<OrganismModel>().type == pruneOrganismType)
                    {
                        bool isPruning = Random.value < popPercentToPrune;
                        if (isPruning)
                        {
                            Destroy(organism);
                            gameManager.organismGrid[rowIndex][colIndex] = null;
                            Debug.Log("Plant pruned successfully!");
                        }
                    }
                }
            }
        }
    }
}
