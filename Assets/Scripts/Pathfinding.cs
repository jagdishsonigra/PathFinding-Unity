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

    private Tile[,] grid;
    private List<Tile> blockedTiles = new List<Tile>();

    public void InitializeTiles(Tile[,] tiles)
    {
        grid = tiles;
        Debug.Log("Tiles initialized.");
    }

    public void UpdateBlockedTiles(Tile enemyTile)
    {
        blockedTiles.Clear();
        // Add the tile occupied by the enemy to the blocked list
        blockedTiles.Add(enemyTile);
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
                if (openSet[i].FCost < currentTile.FCost || (openSet[i].FCost == currentTile.FCost && openSet[i].hCost < currentTile.hCost))
                {
                    currentTile = openSet[i];
                }
            }

            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

            if (currentTile == targetTile)
            {
                Debug.Log("Path found from " + startTile + " to " + targetTile);
                return RetracePath(startTile, targetTile);
            }

            foreach (Tile neighbor in GetNeighbors(currentTile))
            {
                if (closedSet.Contains(neighbor) || neighbor.isBlocked || neighbor.IsOccupied || blockedTiles.Contains(neighbor))
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

        Debug.Log("No path available from " + startTile + " to " + targetTile);
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

        if (tile.x > 0)
        {
            neighbors.Add(grid[tile.x - 1, tile.y]);
        }
        if (tile.x < grid.GetLength(0) - 1)
        {
            neighbors.Add(grid[tile.x + 1, tile.y]);
        }
        if (tile.y > 0)
        {
            neighbors.Add(grid[tile.x, tile.y - 1]);
        }
        if (tile.y < grid.GetLength(1) - 1)
        {
            neighbors.Add(grid[tile.x, tile.y + 1]);
        }

        return neighbors;
    }
}
