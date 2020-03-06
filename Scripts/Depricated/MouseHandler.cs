using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseHandler : MonoBehaviour
{
    // Runtime variables
    public Vector2Int boardSize = new Vector2Int(15, 15);
    public int playerMovementRange = 3;
    public int playerAttackRange = 2;

    // Exposed variables to link objects without having to use code
    public TileBase whiteTile;
    public TileBase redTile;
    public TileBase greenTile;
    public TileBase blueTile;
    public Tilemap background;
    public Tilemap gui;

    // Start is called before the first frame update
    void Start()
    {
        // Move and scale the camera depending on the board size
        Camera.main.transform.position = new Vector3(boardSize.x / 2, boardSize.y / 2, -10);
        Camera.main.orthographicSize = Math.Max(boardSize.x, boardSize.y) * 0.55f;

        // Fill the board with background tiles
        for (int i = 0; i < boardSize.x; i++)
        {
            for (int j = 0; j < boardSize.y; j++)
            {
                background.SetTile(new Vector3Int(i, j, 0), whiteTile);
            }
        }
        DisplayGUITiles(new Vector3Int(boardSize.x / 2, boardSize.y / 2, 0), playerMovementRange, playerAttackRange);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get the mouse position in world coordinates
            Vector3 mousePositionInGridCoordinates = gui.WorldToCell(mousePosition); // Convert coordinates to cell space
            Vector3Int tilePosition = new Vector3Int(Mathf.FloorToInt(mousePositionInGridCoordinates.x), Mathf.FloorToInt(mousePositionInGridCoordinates.y), 0); // Make a Vector3Int to use as cell indeces
            if (gui.GetTile(tilePosition) && gui.GetTile(tilePosition).name == blueTile.name)
            {
                DisplayGUITiles(tilePosition, playerMovementRange, playerAttackRange);
            }
        }
    }

    public void DisplayGUITiles(Vector3Int origin, int movementRange, int attackRange)
    {
        // Clear old tiles and fill using red for attack range, blue for movement, and green for character position
        if ((origin.x >= 0 && origin.x < boardSize.x && origin.y >= 0 && origin.y < boardSize.y))
        {
            gui.ClearAllTiles();
            int totalRange = movementRange + attackRange;
            Vector3Int currentTilePosition = origin;
            for (int i = totalRange; i >= -totalRange; i--)
            {
                currentTilePosition.y = origin.y + i;
                int lateralRange = totalRange - Math.Abs(i);
                for (int j = lateralRange; j >= -lateralRange; j--)
                {
                    currentTilePosition.x = origin.x + j;
                    TileBase tile = blueTile;
                    if (Math.Abs(i) + Math.Abs(j) > movementRange)
                    {
                        tile = redTile;
                    }
                    if (currentTilePosition.x >= 0 && currentTilePosition.x < boardSize.x && currentTilePosition.y >= 0 && currentTilePosition.y < boardSize.y)
                        gui.SetTile(currentTilePosition, tile);
                }
            }
            gui.SetTile(origin, greenTile);
        }
    }

}