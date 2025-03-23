using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayer : MonoBehaviour
{
       // Grid size
    private int gridSizeX = 3;
    private int gridSizeY = 3;
    
    private int secondGridSizeX = 1;
    private int secondGridSizeY = 3;
    // Grid to represent occupied cells (true means occupied, false means empty)
    private bool[,] grid;

    private int currentGrid = 0;
    // Cursor position
    private int currentX = 0;
    private int currentY = 0;
    
    // Array to hold references to the child objects (game objects in the grid)
    private GameObject[,] gridObjects;
    private GameObject[,] gridObjects2;


    void Start()
    {
        // Initialize the grid with false (empty) or true (occupied)
        grid = new bool[gridSizeX, gridSizeY];
        
        
        // Initialize gridObjects array and reference child objects
        gridObjects = new GameObject[gridSizeX, gridSizeY];
        gridObjects2 = new GameObject[secondGridSizeX, secondGridSizeY];

        // Find child objects and assign them based on name (assuming the objects are named 0,1,2,10,11,12,20,21,22)
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                string objectName = $"{i}{j}"; // Names like "00", "01", "02", etc.
                Transform child = transform.Find(objectName);
                if (child != null)
                {
                    gridObjects[i, j] = child.gameObject;

                    // Example: Set the initial color based on the occupied state
                    SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        sr.color = Color.blue; // Set default color
                    }
                }
            }
        }
        // Find child objects and assign them based on name (assuming the objects are named 0,1,2,10,11,12,20,21,22)
        for (int i = 0; i < secondGridSizeX; i++)
        {
            for (int j = 0; j < secondGridSizeY; j++)
            {
                string objectName = $"{j}";
                Transform child = transform.Find(objectName);
                if (child != null)
                {
                    gridObjects2[i, j] = child.gameObject;

                    // Example: Set the initial color based on the occupied state
                    SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        sr.color = Color.green; // Set default color
                    }
                }
            }
        }
        // Example of setting some cells as occupied
        grid[0, 0] = true;  
        grid[0, 1] = true;  
        grid[0, 2] = true;  
        grid[1, 0] = true; 
        grid[1, 1] = false;  
        grid[1, 2] = true;
        grid[2, 0] = true; 
        grid[2, 1] = true;  
        grid[2, 2] = true;

       
    }

    void Update()
    {
        HandleMovement();
        HandleSelect();
    }

    void HandleSelect()
    {
        PriorSelect();
        if (Input.GetKeyDown(KeyCode.Return))
        {
            
        }

        
        else return;
        // Visualize cursor position
        if (currentGrid == 0)
        {
            currentGrid = 1;
            currentX=0; 
            currentY=0;
            UpdateGridVisuals();
        }
        Debug.Log($"Cursor Position: ({currentX}, {currentY})");
    }

    void PriorSelect()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            
        }
        else return;

        if (currentGrid == 1)
        {
            currentGrid = 0;
            currentX = 0;
            currentY = 0;
        }
        UpdateGridVisuals();
    }

    void HandleMovement()
    {
        if (currentGrid == 0)  // Second Grid Selected
        {
            // Move in the second grid
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                AttemptMove(currentX, currentY - 1, "up");
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                AttemptMove(currentX, currentY + 1, "down");
            }
        }
        else if (currentGrid == 1)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                AttemptMove(currentX, currentY - 1, "up");
            }

            // Move Down
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                AttemptMove(currentX, currentY + 1, "down");
            }

            // Move Left
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                AttemptMove(currentX - 1, currentY, "left");
            }

            // Move Right
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                AttemptMove(currentX + 1, currentY, "right");
            }
        }

        

        // Move Up
    }

    // Attempt to move to the target position
    void AttemptMove(int targetX, int targetY, string direction)
    {
        // First check if the target position is in bounds
        if (IsInBounds(targetX, targetY))
        {
            // Check if the target cell is occupied
            if (grid[targetX, targetY])
            {
                // Move to the target cell if it's occupied
                currentX = targetX;
                currentY = targetY;
            }
            else
            {
                // If the target cell is empty, find the nearest occupied cell in that direction
                MoveToClosestOccupied(targetX, targetY, direction);
            }
            UpdateGridVisuals();
        }
    }

    // Check if a cell is within grid bounds
    bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY;
    }

    // Move to the closest occupied cell in a specific direction
    void MoveToClosestOccupied(int targetX, int targetY, string direction)
    {
        // Based on the direction of movement, check neighboring cells
        switch (direction)
        {
            case "up":
                if (IsInBounds(targetX + 1, targetY) && grid[targetX + 1, targetY])
                {
                    // Check below
                    currentX = targetX + 1;
                    currentY = targetY;
                }
                else if (IsInBounds(targetX - 1, targetY) && grid[targetX - 1, targetY])
                {
                    // Check left
                    currentX = targetX - 1;
                    currentY = targetY;
                }
                break;

            case "down":
                if (IsInBounds(targetX - 1, targetY) && grid[targetX - 1, targetY])
                {
                    // Check right
                    currentX = targetX - 1;
                    currentY = targetY;
                }
                else if (IsInBounds(targetX + 1, targetY) && grid[targetX + 1, targetY])
                {
                    // Check below
                    currentX = targetX + 1;
                    currentY = targetY;
                }
                break;

            case "left":
                if (IsInBounds(targetX, targetY - 1) && grid[targetX, targetY - 1])
                {
                    // Check below
                    currentX = targetX;
                    currentY = targetY - 1;
                }
                else if (IsInBounds(targetX, targetY + 1) && grid[targetX, targetY + 1])
                {
                    // Check above
                    currentX = targetX;
                    currentY = targetY + 1;
                }
                break;

            case "right":
                if (IsInBounds(targetX, targetY - 1) && grid[targetX, targetY - 1])
                {
                    // Check above
                    currentX = targetX;
                    currentY = targetY - 1;
                }
                else if (IsInBounds(targetX, targetY + 1) && grid[targetX, targetY + 1])
                {
                    // Check below
                    currentX = targetX;
                    currentY = targetY + 1;
                }
                break;
        }
        
        // If no occupied cell found in that direction, remain in the same spot
    }
    
    void UpdateGridVisuals()
    {
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                SpriteRenderer sr = gridObjects[i, j].GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    if(currentGrid == 0) sr.color = Color.blue;
                    // Change color based on whether it's the selected position
                    else if (i == currentX && j == currentY)
                    {
                        sr.color = Color.red; // Highlight selected position
                    }
                    else if (grid[i, j] == false)
                    {
                        sr.color = Color.black; // Default color for non-selected
                    }
                    else
                    {
                        sr.color = Color.blue;
                    }
                }
                
            }
        }
        for (int i = 0; i < secondGridSizeX; i++)
        {
            for (int j = 0; j < secondGridSizeY; j++)
            {
                SpriteRenderer sr2 = gridObjects2[i, j].GetComponent<SpriteRenderer>();
                if (sr2 != null)
                {
                    
                    if(currentGrid == 1) sr2.color = Color.green;
                    // Change color based on whether it's the selected position
                    else if (i == currentX && j == currentY)
                    {
                        sr2.color = Color.red; // Highlight selected position
                    }
                    else
                    {
                        sr2.color = Color.green;
                    }
                    
                        
                }
                
            }
        }
        
    }
}
