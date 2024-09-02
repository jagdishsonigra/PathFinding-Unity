using UnityEngine;
using TMPro;

public class TileInfoDisplay : MonoBehaviour
{
    public TMP_Text tileInfoText; // Reference to TextMesh Pro text element
    public Camera mainCamera; // Reference to the main camera

    public LayerMask tileLayerMask; // Layer mask for tile layer

    private void Update()
    {
        UpdateTileInfo(); // Update tile info on every frame
    }

    // Update the text to display information about the tile being hovered over
    void UpdateTileInfo()
    {
        // Cast a ray from the mouse position into the game world
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform the raycast using the tileLayerMask
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayerMask))
        {
            Tile tile = hit.transform.GetComponent<Tile>();
            if (tile != null)
            {
                // Check if the tile is walkable and update the text accordingly
                tileInfoText.text = tile.IsWalkable
                    ? $"Tile Position: ({tile.x}, {tile.y})\nWalkable: Yes"
                    : $"Tile Position: ({tile.x}, {tile.y})\nWalkable: No";
            }
            else
            {
                // Hit something that is not a tile
                tileInfoText.text = "Not a tile";
            }
        }
        else
        {
            // No tile was found at the mouse position
            tileInfoText.text = "Hover over a tile";
        }
    }
}
