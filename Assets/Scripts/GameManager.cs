using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public int numRows = 10, numCols = 10, numInitialOrganisms = 5;

    public GameObject tilePrefab;
    public List<GameObject> organismPrefabs;

    public float simulationUpdatePeriodSec = 1.0f;

    public float reproductionProbability = 0.1f;

    private List<List<GameObject>> generatedTiles;
    private List<GameObject> organisms;
    private List<List<GameObject>> organismGrid;
    private float lastSimulationUpdateSec;

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

            List<GameObject> rowList = new List<GameObject>();
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
        organisms = new List<GameObject>();
        for (int i = 0; i < numInitialOrganisms; i++)
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


            GameObject prefabToInstantiate = organismPrefabs[Random.Range(0, organismPrefabs.Count)];
            GameObject newOrganism = Instantiate(prefabToInstantiate, organismPosition, defaultRotation);
            organisms.Add(newOrganism);
            organismGrid[rowIndex][colIndex] = newOrganism;
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool isTimeToUpdateSimulation = Time.time - lastSimulationUpdateSec > simulationUpdatePeriodSec;
        if (isTimeToUpdateSimulation)
        {
            // Make a copy of the organism list so we can mutate the original while looping
            List<GameObject> organismCopy = new List<GameObject>(organisms);
            foreach (GameObject org in organismCopy)
            {
                // The organism may have been eaten in a previous update
                if (org is null)
                {
                    continue;
                }

                bool isReproducing = Random.value < reproductionProbability;
                if (isReproducing)
                {
                    Vector3 parentPosition = org.transform.position;

                    int rowIndex = (int) parentPosition.x;
                    int colIndex = (int) parentPosition.z;

                    rowIndex += Random.Range(-1, 2);
                    colIndex += Random.Range(-1, 2);

                    Vector3 childPosition = new Vector3(rowIndex, parentPosition.y, colIndex);

                    bool isWithinWorldBounds =
                        0 <= childPosition.x && childPosition.x < numRows
                        && 0 <= childPosition.z && childPosition.z < numCols;
                    if (!isWithinWorldBounds)
                    {
                        continue;
                    }

                    bool positionHasExistingOrganism = !(organismGrid[rowIndex][colIndex] is null);
                    if (positionHasExistingOrganism)
                    {
                        GameObject existingOrganism = organismGrid[rowIndex][colIndex];
                        OrganismType existingOrganismType = existingOrganism.GetComponent<OrganismModel>().type;

                        bool parentTypeCanEatExistingOrg =
                            org.GetComponent<OrganismModel>().CanEatType(existingOrganismType);

                        if (parentTypeCanEatExistingOrg)
                        {
                            Destroy(existingOrganism);
                            organismGrid[rowIndex][colIndex] = null;
                        }
                        else {
                            continue;
                        }
                    }

                    Quaternion defaultRotation = new Quaternion();
                    GameObject newOrganism = Instantiate(org, childPosition, defaultRotation);
                    organisms.Add(newOrganism);
                    organismGrid[rowIndex][colIndex] = newOrganism;
                }
            }
        }
    }
}
