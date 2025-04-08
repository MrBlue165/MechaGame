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
    private List<string> playerAbilities = new List<string>();
    private List<string> playerSpecials = new List<string>();
    private List<Transform> childs = new List<Transform>();
    
    Entity player = null;
    
    // Grid to represent occupied cells (true means occupied, false means empty)
    private bool[,] grid;

    enum GameState
    {
        PlayerTurn,
        EnemyTurn
    }
    
    GameState currentGameState;
    private int currentGrid = 0;
    private int selection = 0;
    // Cursor position
    private int currentX = 0;
    private int currentY = 0;
    
    // Array to hold references to the child objects (game objects in the grid)
    private GameObject[,] gridObjects;
    private GameObject[,] gridObjects2;


    void Start()
    {
        currentGameState = GameState.PlayerTurn;
        Entity enemy = EnemyFind();
        enemy.abilities.Add(new Ability("Cannon Shot", 5, 5, 0, 0.8));
        
        
        player = FindPlayer();
        if(player != null)
        {  
            player.abilities.Add(new Ability("Cannon Shot", 5, 5, 0, 0.8));
            player.abilities.Add(new Ability("Minigun", 4, 6, 2, 1));
            foreach (var ability in player.abilities)
            {
                playerAbilities.Add(ability.abilityName);
            }
            player.abilities.Add(new Ability("Heal", -5, 3, 0, 1));
            player.abilities.Add(new Ability("Tank Fuel", 0, -5, 0, 1));
            playerSpecials.Add("Heal");
            playerSpecials.Add("Tank Fuel");
            
        }

        
        // Initialize the grid with false (empty) or true (occupied)
        grid = new bool[gridSizeX, gridSizeY];
        
        grid[0,0] = true;
        grid[0,1] = true;
        // Initialize gridObjects array and reference child objects
        gridObjects = new GameObject[gridSizeX, gridSizeY];
        gridObjects2 = new GameObject[secondGridSizeX, secondGridSizeY];

        InitializeGridObjects(gridSizeX, gridSizeY, gridObjects, Color.blue, true);
        InitializeGridObjects(secondGridSizeX, secondGridSizeY, gridObjects2, Color.green, false);
        UpdateGridVisuals();
    }
    
    private void InitializeGridObjects(int gridSizeX, int gridSizeY, GameObject[,] gridObjects, Color defaultColor, bool isOne)
    {
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                string objectName = $"{j}";
                if (isOne == true) objectName = $"{i}{j}";
                Transform child = transform.Find(objectName);
                if (isOne == true) childs.Add(child);
                
                
                if (child != null)
                {
                    gridObjects[i, j] = child.gameObject;

                    SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        sr.color = defaultColor; // Set color based on the passed parameter
                    }

                    // Optionally display ability name in Grid 2
                    
                }
            }
        }
        if (player != null && isOne == true)
        {
            AbilityTextGrid();
        }
        else
        {
            
        }
        
    }
    
    private void AbilityTextGrid()
    {
        int count = 0;
        if (currentGrid == 0)
        {
            foreach (var child in childs)
            {
                if (child != null)
                {
                    TMPro.TextMeshProUGUI text = child.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                    if (text != null)
                    {
                        text.text = "";
                    }
                }
                
            }
        }
        else
        {
            foreach (var child in childs)
            {
               string childName = child.name;
               int x = int.Parse(childName[0].ToString());
               int y = int.Parse(childName[1].ToString());
                                               
               if (child != null && selection == 1)
               {
                   if (count < playerAbilities.Count)
                   {
                       TMPro.TextMeshProUGUI text = child.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                       if (text != null)
                       {
                           text.text = playerAbilities[count];
                                                       
                       }
                                                   
                                              
                       grid[x, y] = true;
                   }
                   else grid[x, y] = false;
               }
               else if (child != null && selection == 0)
               {
                   if (count < playerSpecials.Count)
                   {
                       TMPro.TextMeshProUGUI text = child.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                       if (text != null)
                       {
                           text.text = playerSpecials[count];
                                                       
                       }
                                                   
                                              
                       grid[x, y] = true;
                   }
                   else grid[x, y] = false;
               }
               count++;                                        
                                
                            
            }
        }
        
        
    }


    void Update()
    {
        HandleMovement();
        HandleSelect();
        AbilityTextGrid();
        if (currentGameState == GameState.EnemyTurn)
        {
            EnemyTurn();    
        }
        
    }

    void EnemyTurn()
    {
        var enemy = EnemyFind();
        if (enemy.abilities.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, enemy.abilities.Count);
            var randomAbility = enemy.abilities[randomIndex];

            // Use the randomAbility here
            Debug.Log("Enemy used ability: " + randomAbility.abilityName);
            // You probably want to execute the ability
            enemy.UseAbility(randomAbility, player);
        }
        
        currentGameState = GameState.PlayerTurn;
    }
    void HandleSelect()
    {
        PriorSelect();
        if (Input.GetKeyDown(KeyCode.Return))
        {
            
        }

        
        else return;

        if (currentGrid == 1 && selection == 1)
        {
             Transform child = transform.Find($"{currentX}{currentY}");
                    TMPro.TextMeshProUGUI text = child.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                    //get ability name from text
                    Ability selectedAbility = player.abilities.Find(ability => ability.abilityName == text.text);

                    Entity enemy = EnemyFind();
                    
                    player.UseAbility(selectedAbility, enemy);
                    currentGameState = GameState.EnemyTurn;
        }
        else if (currentGrid == 1 && selection == 0)
        {
            Transform child = transform.Find($"{currentX}{currentY}");
            TMPro.TextMeshProUGUI text = child.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            //get ability name from text
            Ability selectedAbility = player.abilities.Find(ability => ability.abilityName == text.text);
            
            
            player.UseAbility(selectedAbility);
            currentGameState = GameState.EnemyTurn;
        }
        // Visualize cursor position
        if (currentGrid == 0)
        {
            if(currentY == 0) selection = 0;
            else selection = 1;
            currentGrid = 1;
            currentX=0; 
            currentY=0;
            UpdateGridVisuals();
        }
       
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
                if (!(currentY == 1 && playerAbilities.Count < 9))
                {
                    AttemptMove(currentX, currentY + 1, "down");
                }
                
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
                for (int i = 0; i < gridSizeX; i++)
                {
                    if (IsInBounds(targetX + i, targetY) && grid[targetX + i, targetY])
                    {
                        // Check below
                        currentX = targetX + i;
                        currentY = targetY;
                        break;
                    }
                    else if (IsInBounds(targetX - i, targetY) && grid[targetX - i, targetY])
                    {
                        // Check left
                        currentX = targetX - i;
                        currentY = targetY;
                        break;
                    }
                }
                break;

            case "down":
                for (int i = 0; i < gridSizeX; i++)
                {
                    if (IsInBounds(targetX - i, targetY) && grid[targetX - i, targetY])
                    {
                        // Check right
                        currentX = targetX - i;
                        currentY = targetY;
                        break;
                    }
                    else if (IsInBounds(targetX + i, targetY) && grid[targetX + i, targetY])
                    {
                        // Check below
                        currentX = targetX + i;
                        currentY = targetY;
                        break;
                    }
                }
                break;

            case "left":
                for (int i = 0; i < gridSizeY; i++)
                {
                    if (IsInBounds(targetX, targetY - i) && grid[targetX, targetY - i])
                    {
                        // Check below
                        currentX = targetX;
                        currentY = targetY - i;
                        break;
                    }
                    else if (IsInBounds(targetX, targetY + i) && grid[targetX, targetY + i])
                    {
                        // Check above
                        currentX = targetX;
                        currentY = targetY + i;
                        break;
                    }
                }

                break;

            case "right":
                for (int i = 0; i < gridSizeY; i++)
                {
                    if (IsInBounds(targetX, targetY - i) && grid[targetX, targetY - i])
                    {
                        // Check above
                        currentX = targetX;
                        currentY = targetY - i;
                        break;
                    }
                    else if (IsInBounds(targetX, targetY + i) && grid[targetX, targetY + i])
                    {
                        // Check below
                        currentX = targetX;
                        currentY = targetY + i;
                        break;
                    }
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
                    else if(playerAbilities.Count < 9 && j == 2)
                    {
                        sr2.color = Color.black;
                    }
                    else
                    {
                        sr2.color = Color.green;
                    }
                    
                        
                }
                
            }
        }
        
    }
    
    private static Entity FindPlayer()
    {
        // You could search for the player in the scene here, or have a reference.
        // This is just an example of how you might find the player.
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            return playerObject.GetComponent<Entity>();
        }

        return null;
    }
    
    private static Entity EnemyFind()
    {
        // You could search for the player in the scene here, or have a reference.
        // This is just an example of how you might find the player.
        GameObject enemyObject = GameObject.FindWithTag("Enemy");
        if (enemyObject != null)
        {
            return enemyObject.GetComponent<Entity>();
        }

        return null;
    }
}
