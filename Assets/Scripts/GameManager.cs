using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public int numRows = 10, numCols = 10, numInitialOrganisms = 5;

    public GameObject tilePrefab;
    public GameObject organism;

    private List<List<GameObject>> generatedTiles;
    private List<GameObject> organisms;

    void Start()
    {
        InitializeTiles();
        InitializeOrganisms();
    }

    void InitializeTiles()
    {
        generatedTiles = new List<List<GameObject>>();
        for (int rowIndex = 0; rowIndex < numRows; rowIndex++)
        {
            List<GameObject> tileRow = new List<GameObject>();

            List<GameObject> rowList = new List<GameObject>();
            for (int colIndex = 0; colIndex < numCols; colIndex++)
            {
                Vector3 tilePosition = new Vector3(rowIndex, 0, colIndex);
                Quaternion defaultRotation = new Quaternion();
                tileRow.Add(Instantiate(tilePrefab, tilePosition, defaultRotation));
            }

            generatedTiles.Add(tileRow);
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
            organisms.Add(Instantiate(organism, organismPosition, defaultRotation));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
