using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class controlPlayer : MonoBehaviour
{
    public static int playerX; // Stores the current x-coordinate
    public static int playerY; // Stores the current y-coordinate
    public static int playerMoves = 0; // Variable to count player moves

    // Start is called before the first frame update
    void Start()
    {
        // Access setSceneLVL1.levelLayout to find the tile with value 2 (Start tile)
        for (int y = 0; y < setSceneLVL1.levelLayout.GetLength(0); y++)
        {
            for (int x = 0; x < setSceneLVL1.levelLayout.GetLength(1); x++)
            {
                if (setSceneLVL1.levelLayout[y, x] == 2)
                {
                    // Found the Start tile (value 2)
                    // Set the player's initial position to this tile
                    playerX = x;
                    playerY = y;
                    // Move the attached object to this tile's position
                    transform.position = new Vector3(x, -y, -1);
                    return; // Exit the loop once the tile is found
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check for user input (Arrow keys)
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(0, -1); // Move Up
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(1, 0); // Move Right
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(0, 1); // Move Down
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(-1, 0); // Move Left
        }
    }

    // Function to move the player in the specified direction
    private void Move(int deltaX, int deltaY)
    {
        // Calculate the next position based on the current position and desired direction
        int nextX = playerX + deltaX;
        int nextY = playerY + deltaY;

        // Check if the next position is within the bounds of the grid and is not a wall (value 0)
        if (nextX >= 0 && nextX < setSceneLVL1.levelLayout.GetLength(1) && nextY >= 0 && nextY < setSceneLVL1.levelLayout.GetLength(0) && setSceneLVL1.levelLayout[nextY, nextX] != 0)
        {
            // Update the player's current position
            playerX = nextX;
            playerY = nextY;

            // Move the attached object to the next position
            transform.position = new Vector3(nextX, -nextY, -1);

            // Mark the tile as discovered in the discoveredTiles array
            setSceneLVL1.discoveredTiles[nextY, nextX] = 1;

            // Call the MarkDiscover method to update the grid visually
            setSceneLVL1.MarkDiscover();

            // Increment the playerMoves variable
            playerMoves++;

            // Check if the player has reached the "End" tile (value 3)
            if (setSceneLVL1.levelLayout[nextY, nextX] == 3)
            {
                // Load the "Win" scene
                SceneManager.LoadScene("Level 1");
            }
        }
        else if (setSceneLVL1.levelLayout[nextY, nextX] == 0)
        {
            setSceneLVL1.discoveredTiles[nextY, nextX] = 1;
            setSceneLVL1.MarkDiscover();
        }
    }
}
