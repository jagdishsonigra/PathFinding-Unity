using UnityEngine;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class Tile : MonoBehaviour
{
    public int x, y;
    public bool isBlocked = false;
    public bool IsOccupied { get; set; } // Property to indicate if the tile is occupied
    public bool IsWalkable => !isBlocked && !IsOccupied; // Define walkability

    public Tile parent; // For pathfinding

    public int gCost;
    public int hCost;
    public int FCost => gCost + hCost;

    public void SetPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void BlockTile()
    {
        isBlocked = true;
        GetComponent<Renderer>().material.color = Color.red;
    }

    public void UnblockTile()
    {
        isBlocked = false;
        GetComponent<Renderer>().material.color = Color.white;
    }

    public Tile GetNeighborTile(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return GridGenerator.Instance.GetTileAtPosition(x, y + 1);
            case Direction.Down:
                return GridGenerator.Instance.GetTileAtPosition(x, y - 1);
            case Direction.Left:
                return GridGenerator.Instance.GetTileAtPosition(x - 1, y);
            case Direction.Right:
                return GridGenerator.Instance.GetTileAtPosition(x + 1, y);
            default:
                return null;
        }
    }
}
