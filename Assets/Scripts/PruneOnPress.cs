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
    
    /// FOr UI interaction
    public GameObject prune_button;
    public GameObject prune_text;

    public GameObject butterfly_weed_ui;
    public GameObject pine_ui;
    public GameObject creeper_ui;

    private bool apply_butterfly = false;
    private bool apply_creeper = false;
    private bool apply_pine = false;

    private bool apply_prune;

    void Start()
    {
        lastPruneSec = 0;
    }

    // Update is called once per frame
    void Update()
    {

        apply_butterfly = butterfly_weed_ui.GetComponent<clicked_on>().apply_butterfly;
        apply_creeper = creeper_ui.GetComponent<apply_creeper>().apply2creeper;
        apply_pine = pine_ui.GetComponent<apply_pine>().apply2pine;

        apply_prune = prune_button.GetComponent<prune_apply>().apply_prune;

        OrganismType pruneOrganismType = new OrganismType();
        //Identify plant type to prune
        if (apply_pine && apply_prune)
        {
            pruneOrganismType = OrganismType.Blue;
            isPruneTypeSelected = true;
            //Debug.Log("Prune Blue Trees");
        } else if (apply_prune && apply_creeper)
        {
            pruneOrganismType = OrganismType.Green;
            isPruneTypeSelected = true;
            //Debug.Log("Prune Green Vines");
        } else if (apply_prune && apply_butterfly)
        {
            pruneOrganismType = OrganismType.Red;
            isPruneTypeSelected = true;
            //Debug.Log("Prune Red Weeds");
        }
        float currentTimeSec = Time.time;
        bool isTimeToPrune = currentTimeSec - lastPruneSec > pruneCooldownPeriodSec;

        if (isTimeToPrune) 
        {
            prune_button.SetActive(true);
        }
        else
        {
            prune_button.SetActive(false);
        }


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
                            prune_button.GetComponent<prune_apply>().apply_prune = false;
                            //Debug.Log("Plant pruned successfully!");
                        }
                    }
                }
            }
        }
    }
}
