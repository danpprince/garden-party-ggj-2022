using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int numRows = 10, numCols = 10, numInitialOrganismsPerPrefab = 2;

    public GameObject tilePrefab;
    public List<GameObject> organismPrefabs;

    public float simulationUpdatePeriodSec = 1.0f;
    public bool isWatering = false;
    public string currentWeatherCondition = "sunny";

    public float positionJitterAmount;
    public AudioSource waterAudio;

    private List<List<GameObject>> generatedTiles;
    private List<List<GameObject>> organismGrid;
    private float lastSimulationUpdateSec;

    private OrganismType typeBeingWatered;

    // Start is called before the first frame update
    void Start()
    {
        InitializeTiles();
        InitializeOrganisms();
        InvokeRepeating("SetWeatherCondition", 5.0f, 5.0f);

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

                // Add some jitter to the position to make the layout more organic
                Vector3 organismPosition = new Vector3(
                    rowIndex + Random.Range(-1 * positionJitterAmount, positionJitterAmount),
                    0,
                    colIndex + Random.Range(-1 * positionJitterAmount, positionJitterAmount)
                );

                bool organismExistsAtPosition = !(organismGrid[rowIndex][colIndex] is null);
                if (organismExistsAtPosition)
                {
                    continue;
                }

                GameObject newOrganism = Instantiate(orgPrefab, organismPosition, orgPrefab.transform.rotation);
                newOrganism.transform.Rotate(Vector3.up, Random.Range(0.0f, 360.0f), Space.World);
                newOrganism.transform.localScale = Random.Range(0.7f, 1.0f) * Vector3.one;
                OrganismModel model = newOrganism.GetComponent<OrganismModel>();
                model.rowIndex = rowIndex;
                model.colIndex = colIndex;
                organismGrid[rowIndex][colIndex] = newOrganism;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSimulation();

        if (!isWatering)
        {
            // butterfly weed
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StartCoroutine(WateringCoroutine(OrganismType.Red));
            }

            // virginia creeper
            else if (Input.GetKeyDown(KeyCode.W))
            {
                StartCoroutine(WateringCoroutine(OrganismType.Green));
            }

            // pine
            else if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(WateringCoroutine(OrganismType.Blue));
            }
        }
    }

    void SetWeatherCondition()
    {
      // ugly code
      // sunny 40%
      // cloudy 30%
      // rainy 20%
      // frost 10%

      int result = Random.Range(0, 101);

      if (result > 0 && result <= 40) {
        currentWeatherCondition = "sunny";
      }

      if (result > 40 && result <= 70) {
        currentWeatherCondition = "cloudy";
      }

      if (result > 70 && result <= 90) {
        currentWeatherCondition = "rainy";
      }

      if (result > 90 && result <= 100) {
        currentWeatherCondition = "frost";
      }

      print("===========[ currentWeatherCondition ]=============");
      print(currentWeatherCondition);
    }

    float WeatherModifier(object type)
    {
      // ugly code
      // green - virginia creeper
      // red - butterfly weed
      // blue - long leaf pine

      if (currentWeatherCondition == "sunny")
      {
        if (type.ToString() == "Red")
        {
          return 0.03f;
        }

        if (type.ToString() == "Blue")
        {
          return 0.02f;
        }

        if (type.ToString() == "Green")
        {
          return 0.01f;
        }
      }

      if (currentWeatherCondition == "cloudy")
      {
        return -0.02f;
      }

      if (currentWeatherCondition == "rainy")
      {
        if (type.ToString() == "Blue")
        {
          return 0.03f;
        }

        if (type.ToString() == "Green")
        {
          return 0.02f;
        }

        if (type.ToString() == "Red")
        {
          return 0.01f;
        }
      }

      if (currentWeatherCondition == "frost")
      {
        if (type.ToString() == "Blue")
        {
          return 0.0f;
        }

        return -0.05f;
      }

      return 0.0f;
    }

    IEnumerator WateringCoroutine(OrganismType type)
    {
        isWatering = true;
        typeBeingWatered = type;
        Debug.LogFormat("Watering type {0}...", type.ToString());

        waterAudio.Play();

        yield return new WaitForSeconds(2);

        waterAudio.Stop();

        isWatering = false;
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
                    float reproductionProbability = organismModel.reproductionProbability(isTypeBeingWatered, + WeatherModifier(organismModel.type));

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
        OrganismModel parentModel = organism.GetComponent<OrganismModel>();
        int childRowIndex = parentModel.rowIndex;
        int childColIndex = parentModel.colIndex;

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

        // Add some jitter to the position to make the layout more organic
        Vector3 childPosition = new Vector3(
            childRowIndex + Random.Range(-1 * positionJitterAmount, positionJitterAmount),
            0,
            childColIndex + Random.Range(-1 * positionJitterAmount, positionJitterAmount)
        );

        GameObject childOrganism = Instantiate(organism, childPosition, organism.transform.rotation);
        childOrganism.transform.Rotate(Vector3.up, Random.Range(0.0f, 360.0f), Space.World);
        childOrganism.transform.localScale = Random.Range(0.7f, 1.0f) * Vector3.one;
        organismGrid[childRowIndex][childColIndex] = childOrganism;
        OrganismModel childModel = childOrganism.GetComponent<OrganismModel>();
        childModel.rowIndex = childRowIndex;
        childModel.colIndex = childColIndex;
    }
}
