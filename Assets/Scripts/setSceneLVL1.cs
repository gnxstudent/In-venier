using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class setSceneLVL1 : MonoBehaviour
{
    public static int[,] levelLayout; // Declare the level layout as a public static variable
    public static int[,] discoveredTiles; // Declare the discovered tiles array
    public static GameObject[,] gridTiles; // Array to store references to grid tiles

    public static GameObject Victory;

    [SerializeField] private TextAsset layoutLevel; // The text file containing the level layout
    [SerializeField] private GameObject tilePrefab; // The prefab to instantiate
    [SerializeField] private Camera mainCamera; // Reference to the main camera

    // Start is called before the first frame update
    void Start()
    {
        if (layoutLevel != null && tilePrefab != null)
        {
            string[] lines = layoutLevel.text.Split('\n');

            if (lines.Length >= 3)
            {
                levelLayout = new int[lines.Length, lines[1].Length - 1]; // This is a wierd bug where if there are more than two rows, it creates an extra collumn
                discoveredTiles = new int[lines.Length, lines[1].Length - 1]; 
                gridTiles = new GameObject[lines.Length, lines[1].Length - 1]; 
            }
            else
            {
                levelLayout = new int[lines.Length, lines[1].Length];
                discoveredTiles = new int[lines.Length, lines[1].Length];
                gridTiles = new GameObject[lines.Length, lines[1].Length];
            }

            // Loop through each line of the layout file
            for (int y = 0; y < lines.Length; y++)
            {
                string line = lines[y].Trim();

                // Loop through each character in the line
                for (int x = 0; x < line.Length; x++)
                {
                    char tileChar = line[x];

                    // Map characters to numbers and store them in 'levelLayout'
                    switch (tileChar)
                    {
                        case 'X':
                            levelLayout[y, x] = 0; // Wall
                            break;
                        case 'O':
                            levelLayout[y, x] = 1; // Open
                            break;
                        case 'S':
                            levelLayout[y, x] = 2; // Start
                            break;
                        case 'E':
                            levelLayout[y, x] = 3; // End
                            break;
                        default:
                            // Handle unknown characters or errors as needed
                            break;
                    }

                    // Initialize 'discoveredTiles' with zeros
                    discoveredTiles[y, x] = 0;
                }
            }

            // Now you have 'levelLayout' and 'discoveredTiles' arrays with the same dimensions

            // Call the CreateGrid function to instantiate the grid
            CreateGrid();

            // Call CalculateAndMoveCamera to move the camera to the center of the grid
            CalculateAndMoveCamera();
        }
        else
        {
            Debug.LogError("TextAsset, tilePrefab, or mainCamera is not set correctly!");
        }
    }

    // Function to instantiate the grid
    void CreateGrid()
    {
        for (int y = 0; y < levelLayout.GetLength(0); y++)
        {
            for (int x = 0; x < levelLayout.GetLength(1); x++)
            {
                int tileType = levelLayout[y, x];
                GameObject tile = Instantiate(tilePrefab, new Vector3(x, -y, 0), Quaternion.identity);

                // Change the color based on the tileType
                switch (tileType)
                {
                    /*case 0: // Blue for 0
                        tile.GetComponent<Renderer>().material.color = Color.blue;
                        break;
                    case 1: // Red for 1
                        tile.GetComponent<Renderer>().material.color = Color.red;
                        break;*/
                    case 2: // Green for 2 THIS IS NOW USED SO THAT THE SQUARE UNDER THE PLAYER AT START IS ALSO MAGENTA
                        tile.GetComponent<Renderer>().material.color = Color.magenta;
                        break;
                    case 3: // Yellow for 3
                        Victory = tile;
                        break;
                    /*default:
                        // Handle other cases or errors as needed
                        break;*/
                }

                // Store the reference to the instantiated tile in the gridTiles array
                gridTiles[y, x] = tile;
            }
        }
    }

    // Function to calculate and move the camera to the center of the grid
    void CalculateAndMoveCamera()
    {
        if (mainCamera != null)
        {
            int gridWidth = levelLayout.GetLength(1);
            int gridHeight = levelLayout.GetLength(0);

            // Calculate the center position of the grid
            Vector3 gridCenter = new Vector3((gridWidth - 1) / 2.0f, -(gridHeight - 1) / 2.0f, -10);

            // Move the camera to the calculated grid center
            mainCamera.transform.position = gridCenter;

            // Optional: You may want to adjust the camera's depth or orthographic size here
        }
        else
        {
            Debug.LogError("Main camera is not set!");
        }
    }

    // Static public function to mark discovered tiles and change their colors to pink
    public static void MarkDiscover()
    {
        for (int y = 0; y < discoveredTiles.GetLength(0); y++)
        {
            for (int x = 0; x < discoveredTiles.GetLength(1); x++)
            {
                // Check if the tile is discovered (has a value of 1) in the discoveredTiles array
                if (discoveredTiles[y, x] == 1)
                {
                    // Change the color of the corresponding tile in the gridTiles array to pink
                    if (gridTiles[y, x] != null)
                    {
                        gridTiles[y, x].GetComponent<Renderer>().material.color = Color.magenta;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
