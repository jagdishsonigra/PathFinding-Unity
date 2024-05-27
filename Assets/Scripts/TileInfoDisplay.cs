using UnityEngine;
using TMPro;

public class TileInfoDisplay : MonoBehaviour
{
    public TMP_Text tileInfoText; // Reference to TextMesh Pro text element

    void Update()
    {
        UpdateTileInfo(); // Update tile info on every frame
    }

    // Update the text to display information about the tile being hovered over
    void UpdateTileInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Tile tile = hit.transform.GetComponent<Tile>();
            if (tile != null)
            {
                // Check if the tile is blocked and update the text accordingly
                if (tile.isBlocked)
                {
                    tileInfoText.text = $"Tile Position: Blocked";
                }
                else
                {
                    tileInfoText.text = $"Tile Position: ({tile.x}, {tile.y})";
                }
            }
        }
        else
        {
            tileInfoText.text = "Hover over a tile";
        }
    }
}
