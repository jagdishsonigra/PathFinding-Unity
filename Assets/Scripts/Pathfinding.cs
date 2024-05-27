using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private static Pathfinding instance;
    public static Pathfinding Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Pathfinding>();
                if (instance == null)
                {
                    Debug.LogError("Pathfinding instance not found in the scene.");
                }
            }
            return instance;
        }
    }

    private Tile[,] grid; // 2D array to store references to tiles in the grid

    public void InitializeTiles(Tile[,] tiles)
    {
        grid = tiles;
    }

    public List<Tile> FindPath(Tile startTile, Tile targetTile)
    {
        List<Tile> openSet = new List<Tile>();
        HashSet<Tile> closedSet = new HashSet<Tile>();

        openSet.Add(startTile);

        while (openSet.Count > 0)
        {
            Tile currentTile = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentTile.FCost || openSet[i].FCost == currentTile.FCost && openSet[i].hCost < currentTile.hCost)
                {
                    currentTile = openSet[i];
                }
            }

            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

            if (currentTile == targetTile)
            {
                return RetracePath(startTile, targetTile);
            }

            foreach (Tile neighbor in GetNeighbors(currentTile))
            {
                if (closedSet.Contains(neighbor) || neighbor.isBlocked)
                {
                    continue;
                }

                int newMovementCostToNeighbor = currentTile.gCost + GetDistance(currentTile, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetTile);
                    neighbor.parent = currentTile;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return null; // No path found
    }

    List<Tile> RetracePath(Tile startTile, Tile endTile)
    {
        List<Tile> path = new List<Tile>();
        Tile currentTile = endTile;

        while (currentTile != startTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.parent;
        }
        path.Reverse();
        return path;
    }

    int GetDistance(Tile tileA, Tile tileB)
    {
        int dstX = Mathf.Abs(tileA.x - tileB.x);
        int dstY = Mathf.Abs(tileA.y - tileB.y);
        return dstX + dstY;
    }

    List<Tile> GetNeighbors(Tile tile)
    {
        List<Tile> neighbors = new List<Tile>();

        if (tile.x > 0 && !IsEnemyOccupied(tile.x - 1, tile.y)) // Left
        {
            neighbors.Add(grid[tile.x - 1, tile.y]);
        }
        if (tile.x < grid.GetLength(0) - 1 && !IsEnemyOccupied(tile.x + 1, tile.y)) // Right
        {
            neighbors.Add(grid[tile.x + 1, tile.y]);
        }
        if (tile.y > 0 && !IsEnemyOccupied(tile.x, tile.y - 1)) // Down
        {
            neighbors.Add(grid[tile.x, tile.y - 1]);
        }
        if (tile.y < grid.GetLength(1) - 1 && !IsEnemyOccupied(tile.x, tile.y + 1)) // Up
        {
            neighbors.Add(grid[tile.x, tile.y + 1]);
        }

        return neighbors;
    }

    bool IsEnemyOccupied(int x, int y)
    {
        // Check if there is an enemy at the specified position
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy != null)
        {
            Vector3 enemyPos = enemy.transform.position;
            int enemyX = Mathf.RoundToInt(enemyPos.x);
            int enemyY = Mathf.RoundToInt(enemyPos.z);
            if (enemyX == x && enemyY == y)
            {
                return true;
            }
        }
        return false;
    }
}
