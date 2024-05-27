using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public ObstacleData obstacleData; // Reference to the ScriptableObject containing obstacle positions
    public GameObject obstaclePrefab; // Prefab for the obstacle (e.g., a red sphere)
    private Tile[,] tiles; // 2D array to store references to tiles in the grid

    // This method is called from the GridGenerator after the grid is fully created
    public void InitializeTiles()
    {
        // Initialize the tiles array
        tiles = new Tile[10, 10];
        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            tiles[tile.x, tile.y] = tile;
            Debug.Log($"Tile found at position ({tile.x}, {tile.y})");
        }
    }

    // Public method to generate obstacles, called after the grid is generated
    public void GenerateObstacles()
    {
        if (obstacleData == null || obstaclePrefab == null)
        {
            Debug.LogError("ObstacleData or ObstaclePrefab is not assigned.");
            return;
        }

        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                int index = y * 10 + x;
                if (obstacleData.obstaclePositions[index])
                {
                    Vector3 position = new Vector3(x, 0.5f, y); // Position slightly above the tile
                    GameObject obstacle = Instantiate(obstaclePrefab, position, Quaternion.identity);
                    obstacle.tag = "Obstacle"; // Set the tag for later identification
                    Debug.Log($"Placed obstacle at position ({x}, {y})");

                    // Block the corresponding tile
                    if (tiles[x, y] != null)
                    {
                        tiles[x, y].BlockTile();
                    }
                    else
                    {
                        Debug.LogError($"Tile at position ({x}, {y}) is not found.");
                    }
                }
            }
        }
    }

    // Method to remove obstacles and unblock tiles
    public void RemoveObstacles()
    {
        // Destroy all obstacles in the scene
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in obstacles)
        {
            Destroy(obstacle);
        }

        // Unblock all tiles
        foreach (Tile tile in tiles)
        {
            if (tile != null)
            {
                tile.UnblockTile();
            }
        }
    }
}
