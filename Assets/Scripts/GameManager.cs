using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int numRows = 10, numCols = 10, numInitialOrganismsPerPrefab = 2;

    public GameObject tilePrefab;
    public List<GameObject> organismPrefabs;

    public float simulationUpdatePeriodSec = 1.0f;
    public bool isWatering = false;

    private List<List<GameObject>> generatedTiles;
    private List<List<GameObject>> organismGrid;
    private float lastSimulationUpdateSec;

    private OrganismType typeBeingWatered;

    /// For Handling Effect Application\\\
    public GameObject butterfly_weed_ui;
    public GameObject pine_ui;
    public GameObject creeper_ui;
    ///
    private bool apply_butterfly = false;
    private bool apply_creeper = false;
    private bool apply_pine = false;
    ///
    public GameObject water;
    public GameObject prune;
    public GameObject pest;
    ///
    private bool apply_water = false;
    private bool apply_prune = false;
    private bool apply_fire = false;
    ///
    public GameObject water_text;
    public GameObject prune_text;
    public GameObject fire_text;




    // Start is called before the first frame update
    void Start()
    {
        InitializeTiles();
        InitializeOrganisms();

        lastSimulationUpdateSec = 0;

   
    }

    void InitializeTiles()
    {
        generatedTiles = new List<List<GameObject>>();
        organismGrid = new List<List<GameObject>>();

        for (int rowIndex = 0; rowIndex < numRows; rowIndex++)
        {
            List<GameObject> tileRow = new List<GameObject>();
            List<GameObject> organismGridRow = new List<GameObject>();

            for (int colIndex = 0; colIndex < numCols; colIndex++)
            {
                Vector3 tilePosition = new Vector3(rowIndex, 0, colIndex);
                GameObject newTile = Instantiate(tilePrefab, tilePosition, tilePrefab.transform.rotation);
                tileRow.Add(newTile);

                organismGridRow.Add(null);
            }

            generatedTiles.Add(tileRow);
            organismGrid.Add(organismGridRow);
        }
    }

    void InitializeOrganisms()
    {
        foreach (GameObject orgPrefab in organismPrefabs)
        {
            for (int i = 0; i < numInitialOrganismsPerPrefab; i++)
            {
                int rowIndex = Random.Range(0, numRows);
                int colIndex = Random.Range(0, numCols);

                Vector3 organismPosition = new Vector3(rowIndex, 0, colIndex);

                bool organismExistsAtPosition = !(organismGrid[rowIndex][colIndex] is null);
                if (organismExistsAtPosition)
                {
                    continue;
                }

                GameObject newOrganism = Instantiate(orgPrefab, organismPosition, orgPrefab.transform.rotation);
                newOrganism.transform.Rotate(Vector3.up, Random.Range(0.0f, 360.0f), Space.World);
                newOrganism.transform.localScale = Random.Range(0.7f, 1.0f) * Vector3.one;
                organismGrid[rowIndex][colIndex] = newOrganism;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSimulation();
        apply_butterfly = butterfly_weed_ui.GetComponent<clicked_on>().apply_butterfly;
        apply_creeper = creeper_ui.GetComponent<apply_creeper>().apply2creeper;
        apply_pine = pine_ui.GetComponent<apply_pine>().apply2pine;
        ///
        apply_water = water.GetComponent<water_click>().apply_water;

        if (!isWatering)
        {
            if (apply_butterfly && apply_water)
            {
                StartCoroutine(WateringCoroutine(OrganismType.Red));
            }
            else if (apply_creeper && apply_water)
            {
                StartCoroutine(WateringCoroutine(OrganismType.Green));
            }
            else if (apply_pine && apply_water)
            {
                StartCoroutine(WateringCoroutine(OrganismType.Blue));
            }
        }

        if (isWatering)
        {
            water.SetActive(false);
        }
    }

    IEnumerator WateringCoroutine(OrganismType type)
    {
        isWatering = true;
        typeBeingWatered = type;
        Debug.LogFormat("Watering type {0}...", type.ToString());

        yield return new WaitForSeconds(5);

        isWatering = false;
        water.SetActive(true);
        water.GetComponent<water_click>().apply_water = false;
        Debug.Log("Stopping watering");
    }

    void UpdateSimulation()
    {
        float currentTimeSec = Time.time;
        bool isTimeToUpdateSimulation = currentTimeSec - lastSimulationUpdateSec > simulationUpdatePeriodSec;
        if (isTimeToUpdateSimulation)
        {
            lastSimulationUpdateSec = currentTimeSec;

            for (int rowIndex = 0; rowIndex < numRows; rowIndex++)
            {
                for (int colIndex = 0; colIndex < numCols; colIndex++)
                {
                    GameObject organism = organismGrid[rowIndex][colIndex];
                    if (organism is null)
                    {
                        continue;
                    }

                    OrganismModel organismModel = organism.GetComponent<OrganismModel>();
                    bool isTypeBeingWatered = isWatering && organismModel.type == typeBeingWatered;
                    float reproductionProbability = organismModel.reproductionProbability(isTypeBeingWatered);

                    bool isReproducing = Random.value < reproductionProbability;
                    if (isReproducing)
                    {
                        ReproduceOrganism(organism);
                    }
                }
            }
        }
    }

    // Try to reproduce nearby the passed organism, or return if not possible
    void ReproduceOrganism(GameObject organism)
    {
        Vector3 parentPosition = organism.transform.position;

        int childRowIndex = (int)parentPosition.x;
        int childColIndex = (int)parentPosition.z;

        childRowIndex += Random.Range(-1, 2);
        childColIndex += Random.Range(-1, 2);

        bool isWithinWorldBounds =
            0 <= childRowIndex && childRowIndex < numRows
            && 0 <= childColIndex && childColIndex < numCols;
        if (!isWithinWorldBounds)
        {
            return;
        }

        bool positionHasExistingOrganism = !(organismGrid[childRowIndex][childColIndex] is null);
        if (positionHasExistingOrganism)
        {
            GameObject existingOrganism = organismGrid[childRowIndex][childColIndex];
            OrganismType existingOrganismType = existingOrganism.GetComponent<OrganismModel>().type;

            bool parentTypeCanEatExistingOrg =
                organism.GetComponent<OrganismModel>().CanEatType(existingOrganismType);

            if (parentTypeCanEatExistingOrg)
            {
                Destroy(existingOrganism);
                organismGrid[childRowIndex][childColIndex] = null;
            }
            else
            {
                return;
            }
        }

        Vector3 childPosition = new Vector3(childRowIndex, parentPosition.y, childColIndex);
        GameObject childOrganism = Instantiate(organism, childPosition, organism.transform.rotation);
        childOrganism.transform.Rotate(Vector3.up, Random.Range(0.0f, 360.0f), Space.World);
        childOrganism.transform.localScale = Random.Range(0.7f, 1.0f) * Vector3.one;
        organismGrid[childRowIndex][childColIndex] = childOrganism;
    }
}