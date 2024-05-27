using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public static GridGenerator Instance { get; private set; }

    public GameObject tilePrefab; // Prefab for each tile
    public GameObject playerPrefab; // Prefab for the player unit
    public GameObject enemyPrefab; // Prefab for the enemy unit
    public int gridSize = 10; // Size of the grid

    private Tile[,] tiles; // Store references to tiles in the grid

    void Awake()
    {
        // Set the static instance to this instance
        Instance = this;
    }

    void Start()
    {
        // Check if Pathfinding instance is properly initialized
        if (Pathfinding.Instance == null)
        {
            Debug.LogError("Pathfinding instance is null. Make sure the Pathfinding script is attached to a GameObject in the scene.");
            return;
        }

        GenerateGrid(); // Generate the grid when the game starts
        InstantiateEnemies(); // Instantiate enemies after generating the grid
    }

    // Method to generate the grid of tiles
    void GenerateGrid()
    {
        // Check if playerPrefab is assigned
        if (playerPrefab == null)
        {
            Debug.LogError("PlayerPrefab is not assigned.");
            return;
        }

        tiles = new Tile[gridSize, gridSize]; // Initialize the tiles array

        // Loop through the grid positions to create each tile
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                // Instantiate a new tile from the tilePrefab at the specified position
                GameObject tileObject = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity);
                tileObject.name = $"Tile_{x}_{y}"; // Name the tile for easy identification
                Tile tileScript = tileObject.GetComponent<Tile>(); // Get the Tile script component

                // Check if the Tile script component exists
                if (tileScript == null)
                {
                    Debug.LogError($"Tile prefab does not contain a Tile script at position ({x}, {y}).");
                    continue; // Skip if Tile script is missing
                }

                tileScript.SetPosition(x, y); // Set the position of the tile in the Tile script
                tiles[x, y] = tileScript; // Store reference to the tile in the grid array
                Debug.Log($"Tile created at position ({x}, {y})");
            }
        }

        // After generating the grid, call the ObstacleManager to place obstacles
        ObstacleManager obstacleManager = FindObjectOfType<ObstacleManager>();
        if (obstacleManager != null)
        {
            obstacleManager.InitializeTiles();
            obstacleManager.GenerateObstacles(); // Generate obstacles after grid creation
        }
        else
        {
            Debug.LogError("ObstacleManager not found in the scene.");
        }

        // Initialize Pathfinding with the generated grid
        Pathfinding.Instance.InitializeTiles(tiles);

        // Instantiate the playerPrefab at the starting position
        GameObject player = Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        PlayerController playerController = player.GetComponent<PlayerController>(); // Get the PlayerController script

        if (player != null && playerController != null)
        {
            playerController.currentTile = tiles[0, 0]; // Set the current tile for the player
            Debug.Log("Player instantiated successfully.");
        }
        else
        {
            Debug.LogError("Failed to instantiate playerPrefab or PlayerController script is missing.");
        }
    }

    // Method to instantiate enemies on the grid
void InstantiateEnemies()
{
    bool enemySpawned = false;

    while (!enemySpawned)
    {
        // Check if an enemy has already been spawned
        if (GameObject.FindGameObjectWithTag("Enemy") != null)
        {
            Debug.Log("An enemy has already been spawned.");
            return;
        }

        // Randomly select a tile position
        int randomX = Random.Range(0, gridSize);
        int randomY = Random.Range(0, gridSize);

        // Get the tile at the random position
        Tile randomTile = GetTileAtPosition(randomX, randomY);

        // Check if the tile exists and is not blocked
        if (randomTile != null && !randomTile.isBlocked)
        {
            // Instantiate an enemy at the random position
            GameObject enemy = Instantiate(enemyPrefab, new Vector3(randomX, 1, randomY), Quaternion.identity);

            // Get the EnemyAI script component
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();

            // Initialize the enemy AI with the random tile and player controller
            if (enemyAI != null)
            {
                enemyAI.Initialize(randomTile, FindObjectOfType<PlayerController>());
                enemySpawned = true; // Set the flag to true to indicate successful enemy spawn
            }
            else
            {
                Debug.LogError("Failed to instantiate enemyPrefab or EnemyAI script is missing.");
            }
        }
    }
}




    // Method to get the tile at a specific position
    public Tile GetTileAtPosition(int x, int y)
    {
        if (x >= 0 && x < gridSize && y >= 0 && y < gridSize)
        {
            return tiles[x, y];
        }
        else
        {
            return null;
        }
    }
}
