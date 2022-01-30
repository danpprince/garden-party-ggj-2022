using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruneOnPress : MonoBehaviour
{
    [SerializeField] KeyCode keyPruneBlue;
    [SerializeField] KeyCode keyPruneRed;
    [SerializeField] KeyCode keyPruneGreen;
    [SerializeField] GameManager gameManager;
<<<<<<< HEAD
    
    private List<GameObject> orgsToPrune;
=======
    [SerializeField] float popPercentToPrune;
<<<<<<< HEAD
    
    private List<GameObject> orgsToPrune;
    private List<int> rowIndexPrune;
    private List<int> colIndexPrune;
>>>>>>> cf.level-reset-button

    [SerializeField] public float pruneCooldownPeriodSec = 3.0f;
=======
    [SerializeField] public float pruneCooldownPeriodSec = 1.0f;
>>>>>>> cf.level-reset-button
    private float lastPruneSec;
    private bool isPruneTypeSelected = false;

    void Start()
    {
<<<<<<< HEAD
<<<<<<< HEAD
=======
        orgsToPrune = new List<GameObject>();
        rowIndexPrune = new List<int>();
        colIndexPrune = new List<int>();
>>>>>>> cf.level-reset-button
=======
>>>>>>> cf.level-reset-button
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
            //Debug.Log("Prune Blue Trees");
        } else if (Input.GetKey(keyPruneGreen))
        {
            pruneOrganismType = OrganismType.Green;
            isPruneTypeSelected = true;
            //Debug.Log("Prune Green Vines");
        } else if (Input.GetKey(keyPruneRed))
        {
            pruneOrganismType = OrganismType.Red;
            isPruneTypeSelected = true;
            //Debug.Log("Prune Red Weeds");
        }
        float currentTimeSec = Time.time;
        bool isTimeToPrune = currentTimeSec - lastPruneSec > pruneCooldownPeriodSec;
<<<<<<< HEAD
        if (isTimeToPrune)
        {
            lastPruneSec = currentTimeSec;
            OrganismType pruneOrganismType = new OrganismType();
            //Identify plant type to prune
            if (Input.GetKey(keyPruneBlue))
            {
                pruneOrganismType = OrganismType.Blue;
<<<<<<< HEAD
            } else if (Input.GetKey(keyPruneGreen))
            {
                pruneOrganismType = OrganismType.Green;
            } else if (Input.GetKey(keyPruneRed))
            {
                pruneOrganismType = OrganismType.Red;
            }

=======
                Debug.Log("Prune Blue Trees");
            } else if (Input.GetKey(keyPruneGreen))
            {
                pruneOrganismType = OrganismType.Green;
                Debug.Log("Prune Green Vines");
            } else if (Input.GetKey(keyPruneRed))
            {
                pruneOrganismType = OrganismType.Red;
                Debug.Log("Prune Red Weeds");
            }

=======
        if (isTimeToPrune && isPruneTypeSelected)
        {            
            isPruneTypeSelected = false;
            lastPruneSec = currentTimeSec;     
>>>>>>> cf.level-reset-button
            //crawl through grid looking for organisms, check the type for pruning
>>>>>>> cf.level-reset-button
            for (int rowIndex = 0; rowIndex < gameManager.numRows; rowIndex++)
            {
                for (int colIndex = 0; colIndex < gameManager.numCols; colIndex++)
                {
                    GameObject organism = gameManager.organismGrid[rowIndex][colIndex];
                    if (organism is null)
                    {
                        continue;
                    }
<<<<<<< HEAD

                }
            }

            

            //Prune %% of population
=======
                    if (organism.GetComponent<OrganismModel>().type == pruneOrganismType)
                    {
                        bool isPruning = Random.value < popPercentToPrune;
                        if (isPruning)
                        {
                            Destroy(organism);
                            gameManager.organismGrid[rowIndex][colIndex] = null;
                            //Debug.Log("Plant pruned successfully!");
                        }
                    }
                }
            }
<<<<<<< HEAD

            //Prune %% of population
            int numToPrune = Mathf.RoundToInt(popPercentToPrune*orgsToPrune.Count);
            for(int i = 0; i < numToPrune; i++)
            {
                int k = Random.Range(0, orgsToPrune.Count);
                if (orgsToPrune[k] is null)
                {
                    i--;
                    Debug.Log("Null encountered during pruning");
                }
                else
                {
                    Destroy(orgsToPrune[k]);
                    gameManager.organismGrid[rowIndexPrune[k]][colIndexPrune[k]] = null;
                    Debug.Log("Plant pruned successfully!");
                }
                
            }
>>>>>>> cf.level-reset-button
=======
>>>>>>> cf.level-reset-button
        }
    }
}
