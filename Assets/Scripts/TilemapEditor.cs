using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapEditor : MonoBehaviour
{
    public Tilemap tilemap;  
    private Vector3Int previousTilePosition;  
    private TileBase selectedTile;  

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);

            selectedTile = tilemap.GetTile(cellPosition);
            if (selectedTile != null)
            {
                previousTilePosition = cellPosition;
            }
        }

        if (Input.GetMouseButton(0) && selectedTile != null)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);

            if (cellPosition != previousTilePosition)
            {
                tilemap.SetTile(previousTilePosition, null);
                tilemap.SetTile(cellPosition, selectedTile);
                previousTilePosition = cellPosition;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            selectedTile = null;
        }
    }
}
