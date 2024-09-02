using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action OnPlayerMoveStopped;
    public static event Action<Tile> OnEnemyMove;

    public static void PlayerMoveStopped()
    {
        Debug.Log("Player move stopped event triggered.");
        OnPlayerMoveStopped?.Invoke();
    }

    public static void EnemyMove(Tile tile)
    {
        Debug.Log($"Enemy move event triggered for tile: ({tile.x}, {tile.y})");
        OnEnemyMove?.Invoke(tile);
    }
}
