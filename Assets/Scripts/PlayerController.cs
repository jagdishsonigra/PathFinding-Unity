using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Tile currentTile; // Reference to the current tile the player is on
    public bool IsMoving { get; private set; } = false; // Flag to indicate whether the player is currently moving

    private void Update()
    {
        // If the player is already moving, don't process further input
        if (IsMoving) return;

        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse position into the game world
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Check if the ray hits a tile
                Tile targetTile = hit.transform.GetComponent<Tile>();
                if (targetTile != null && !targetTile.isBlocked)
                {
                    // If the clicked tile is valid and not blocked, find a path to it
                    List<Tile> path = Pathfinding.Instance.FindPath(currentTile, targetTile);
                    if (path != null)
                    {
                        // If a valid path is found, start moving along the path
                        StartCoroutine(MoveAlongPath(path));
                    }
                }
            }
        }
    }

    // Coroutine to move the player along the provided path
    private IEnumerator MoveAlongPath(List<Tile> path)
    {
        IsMoving = true; // Set the IsMoving flag to true

        // Iterate over each tile in the path
        foreach (Tile tile in path)
        {
            // Calculate the target position for the current tile
            Vector3 targetPosition = new Vector3(tile.x, transform.position.y, tile.y);

            // Move towards the target position until the player reaches the tile
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, 5f * Time.deltaTime);
                yield return null; // Wait for the next frame
            }

            // Ensure the player's position is exactly at the target tile
            transform.position = targetPosition;

            // Update the current tile reference to the new tile
            currentTile = tile;
        }

        // After reaching the destination tile, set the IsMoving flag to false
        IsMoving = false;
    }
}
