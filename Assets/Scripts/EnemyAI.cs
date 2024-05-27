using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Tile currentTile;
    private PlayerController player;
    private bool isMoving = false;

    public void Initialize(Tile initialTile, PlayerController player)
    {
        currentTile = initialTile;
        this.player = player;
        StartCoroutine(MoveTowardsPlayerRoutine());
    }

    IEnumerator MoveTowardsPlayerRoutine()
    {
        while (true)
        {
            // Check if the player is adjacent to the enemy
            if (IsPlayerAdjacent())
            {
                isMoving = false;
                yield return null;
            }
            else
            {
                // If the player is not moving and there is a current tile
                if (!player.IsMoving && currentTile != null && !isMoving)
                {
                    // Find the tile adjacent to the player's current tile
                    Tile targetTile = FindNextTile();
                    if (targetTile != null)
                    {
                        // Move towards the target tile
                        yield return StartCoroutine(MoveToTile(targetTile));
                    }
                }
            }
            yield return null;
        }
    }

    bool IsPlayerAdjacent()
    {
        // Get the player's current tile position
        int playerX = player.currentTile.x;
        int playerY = player.currentTile.y;

        // Check if any of the adjacent tiles are the player's current tile
        return currentTile.GetNeighborTile(Direction.Up) == player.currentTile ||
               currentTile.GetNeighborTile(Direction.Down) == player.currentTile ||
               currentTile.GetNeighborTile(Direction.Left) == player.currentTile ||
               currentTile.GetNeighborTile(Direction.Right) == player.currentTile;
    }

    Tile FindNextTile()
    {
        // Check if the current tile is valid
        if (currentTile == null)
        {
            Debug.LogWarning("Current tile is null.");
            return null;
        }

        // Define the potential adjacent tiles
        Tile[] adjacentTiles = new Tile[4];
        adjacentTiles[0] = currentTile.GetNeighborTile(Direction.Up);
        adjacentTiles[1] = currentTile.GetNeighborTile(Direction.Down);
        adjacentTiles[2] = currentTile.GetNeighborTile(Direction.Left);
        adjacentTiles[3] = currentTile.GetNeighborTile(Direction.Right);

        // Filter out adjacent tiles that are outside the grid bounds or are blocked
        List<Tile> validAdjacentTiles = new List<Tile>(); // Define List<Tile> here
        foreach (var tile in adjacentTiles)
        {
            if (tile != null && !tile.isBlocked)
            {
                validAdjacentTiles.Add(tile);
            }
        }

        // If there are no valid adjacent tiles, return null
        if (validAdjacentTiles.Count == 0)
        {
            return null;
        }

        // Calculate the distances to each valid adjacent tile
        float[] distances = new float[validAdjacentTiles.Count];
        for (int i = 0; i < validAdjacentTiles.Count; i++)
        {
            distances[i] = Vector2.Distance(new Vector2(player.currentTile.x, player.currentTile.y), new Vector2(validAdjacentTiles[i].x, validAdjacentTiles[i].y));
        }

        // Find the closest valid tile to the player
        float minDistance = Mathf.Infinity;
        Tile nextTile = null;
        for (int i = 0; i < validAdjacentTiles.Count; i++)
        {
            if (distances[i] < minDistance)
            {
                minDistance = distances[i];
                nextTile = validAdjacentTiles[i];
            }
        }

        return nextTile;
    }

    IEnumerator MoveToTile(Tile targetTile)
    {
        isMoving = true;

        // Calculate the target position based on the target tile
        Vector3 targetPosition = new Vector3(targetTile.x, transform.position.y, targetTile.y);

        // Move towards the target position
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            // Move smoothly towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 5f * Time.deltaTime);
            yield return null;
        }

        // Set the current tile to the target tile
        currentTile = targetTile;

        isMoving = false; // Set isMoving to false when the enemy reaches the target tile
    }
}
