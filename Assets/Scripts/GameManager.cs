using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int numRows = 10, numCols = 10, numInitialOrganismsPerPrefab = 2;

    public GameObject tilePrefab;
    public List<GameObject> organismPrefabs;

    public float simulationUpdatePeriodSec = 1.0f;
    public bool WaterOrganism = false;
    public bool currentlyWatering = false;

    private List<List<GameObject>> generatedTiles;
    private List<List<GameObject>> organismGrid;
    private float lastSimulationUpdateSec;

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
                Quaternion defaultRotation = new Quaternion();
                tileRow.Add(Instantiate(tilePrefab, tilePosition, defaultRotation));

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

                Vector3 organismPosition = new Vector3(rowIndex, 0.5f, colIndex);
                Quaternion defaultRotation = new Quaternion();

                bool organismExistsAtPosition = !(organismGrid[rowIndex][colIndex] is null);
                if (organismExistsAtPosition)
                {
                    continue;
                }

                GameObject newOrganism = Instantiate(orgPrefab, organismPosition, defaultRotation);
                organismGrid[rowIndex][colIndex] = newOrganism;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSimulation();

        if (Input.GetKeyDown(KeyCode.Space) & !currentlyWatering)
        {
          StartCoroutine(Watering());
        }
    }

    IEnumerator Watering()
    {
        WaterOrganism = true;
        currentlyWatering = true;

        yield return new WaitForSeconds(5);

        WaterOrganism = false;
        currentlyWatering = false;
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

                    float reproductionProbability =
                        organism.GetComponent<OrganismModel>().reproductionProbability(WaterOrganism);

                    bool isReproducing = Random.value < reproductionProbability;
                    if (isReproducing)
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
                            continue;
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
                                continue;
                            }
                        }

                        Quaternion defaultRotation = new Quaternion();
                        Vector3 childPosition = new Vector3(childRowIndex, parentPosition.y, childColIndex);
                        GameObject childOrganism = Instantiate(organism, childPosition, defaultRotation);
                        organismGrid[childRowIndex][childColIndex] = childOrganism;
                    }
                }
            }
        }
    }
}
