using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class ShiblitzTile
{
    public enum TYPE { FLOOR, WALL, DOOR, BONUS };

    public static TileBase floorDarkTile = (TileBase)Resources.Load("Tiles/Floor_Dark_Tile");
    public static TileBase floorLightTile = (TileBase)Resources.Load("Tiles/Floor_Light_Tile");
    public static TileBase wallTile = (TileBase)Resources.Load("Tiles/Wall_Tile");
    public static TileBase doorTile = (TileBase)Resources.Load("Tiles/Door_Tile");
    public static TileBase bonusTile = (TileBase)Resources.Load("Tiles/GreenTileWithBorder");

    public static TileBase inputHighlight = (TileBase)Resources.Load("Tiles/InputTileHighlight");
    public static TileBase enemyInputHighlight = (TileBase)Resources.Load("Tiles/EnemyInputTileHighlight");

    public TYPE type;

    public ShiblitzTile(TYPE type)
    {
        this.type = type;
    }

    public static TileBase getAppropriateFloorTile(Vector2Int position)
    {
        if((position.x + position.y) % 2 == 0)
        {
            return floorDarkTile;
        }
        return floorLightTile;
    }
}
