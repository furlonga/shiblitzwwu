using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonBoard
{
    public Tilemap board;
    public Tilemap spells;
    public Tilemap gui;
    public List<Vector2Int> occupiedSpaces;

    public DungeonBoard()
    {
        GameObject g = (GameObject) GameObject.Instantiate(Resources.Load("Grid"));
        board = g.transform.GetChild(0).GetComponent<Tilemap>();
        spells = g.transform.GetChild(1).GetComponent<Tilemap>();
        gui = g.transform.GetChild(2).GetComponent<Tilemap>();
        occupiedSpaces = new List<Vector2Int>();
    }
    
    public void showFogOfWar()
    {
        for (int i = 0; i < Game.getDungeon().dungeonSize.x; i++)
        {
            for (int j = 0; j < Game.getDungeon().dungeonSize.y; j++)
            {
                Game.getDungeonBoard().board.SetTile(new Vector3Int(i, j, 0), ShiblitzTile.wallTile);
            }
        }
    }
}
