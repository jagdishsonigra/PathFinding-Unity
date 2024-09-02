using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Tile currentTile;
    public bool IsMoving { get; private set; } = false;
    private EnemyAI nearbyEnemy;

    private void OnEnable()
    {
        EventManager.OnPlayerMoveStopped += HandlePlayerMoveStopped;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerMoveStopped -= HandlePlayerMoveStopped;
    }

    private void Update()
    {
        if (IsMoving) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Tile targetTile = hit.transform.GetComponent<Tile>();
                if (targetTile != null && !targetTile.isBlocked && !targetTile.IsOccupied)
                {
                    if (nearbyEnemy != null)
                    {
                        Pathfinding.Instance.UpdateBlockedTiles(nearbyEnemy.CurrentTile);
                    }
                    
                    List<Tile> path = Pathfinding.Instance.FindPath(currentTile, targetTile);
                    if (path != null)
                    {
                        StartCoroutine(MoveAlongPath(path));
                    }
                }
            }
        }
    }

    private IEnumerator MoveAlongPath(List<Tile> path)
    {
        IsMoving = true;
        foreach (Tile tile in path)
        {
            Vector3 targetPosition = new Vector3(tile.x, transform.position.y, tile.y);
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, 5f * Time.deltaTime);
                yield return null;
            }
            transform.position = targetPosition;
            currentTile = tile;
        }
        IsMoving = false;
        EventManager.PlayerMoveStopped();
    }

    public void SetNearbyEnemy(EnemyAI enemy)
    {
        nearbyEnemy = enemy;
    }

    private void HandlePlayerMoveStopped()
    {
        Debug.Log("Player has stopped moving. Triggering events.");
        // Logic to handle player stop moving
    }
}
