using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAI
{
    void Initialize(Tile initialTile, PlayerController player);
}

public class EnemyAI : MonoBehaviour, IAI
{
    public Tile CurrentTile { get; private set; }
    private PlayerController player;
    private bool isMoving = false;

    public void Initialize(Tile initialTile, PlayerController player)
    {
        CurrentTile = initialTile;
        this.player = player;
        StartCoroutine(MoveTowardsPlayerRoutine());
    }

    private IEnumerator MoveTowardsPlayerRoutine()
    {
        player.SetNearbyEnemy(this);
        while (true)
        {
            Pathfinding.Instance.UpdateBlockedTiles(CurrentTile);

            if (IsPlayerAdjacent())
            {
                isMoving = false;
                yield return null;
            }
            else
            {
                if (!player.IsMoving && CurrentTile != null && !isMoving)
                {
                    List<Tile> path = Pathfinding.Instance.FindPath(CurrentTile, player.currentTile);
                    if (path != null && path.Count > 0)
                    {
                        foreach (Tile tile in path)
                        {
                            if (IsPlayerAdjacent())
                            {
                                isMoving = false;
                                yield return null;
                                break;
                            }
                            yield return StartCoroutine(MoveToTile(tile));
                            EventManager.EnemyMove(tile);  // Debug event for enemy move
                        }
                    }
                }
            }

            Pathfinding.Instance.UpdateBlockedTiles(null);
            yield return null;
        }
    }

    private bool IsPlayerAdjacent()
    {
        int playerX = player.currentTile.x;
        int playerY = player.currentTile.y;

        return (CurrentTile.GetNeighborTile(Direction.Up) == player.currentTile ||
                CurrentTile.GetNeighborTile(Direction.Down) == player.currentTile ||
                CurrentTile.GetNeighborTile(Direction.Left) == player.currentTile ||
                CurrentTile.GetNeighborTile(Direction.Right) == player.currentTile);
    }

    private IEnumerator MoveToTile(Tile targetTile)
    {
        isMoving = true;
        Vector3 targetPosition = new Vector3(targetTile.x, transform.position.y, targetTile.y);
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 5f * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition;
        CurrentTile = targetTile;
        isMoving = false;
    }
}
