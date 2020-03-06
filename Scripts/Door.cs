using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door
{
    public Vector2Int location;
    public Neighbor neighbor1;
    public Neighbor neighbor2;
    public bool opened;

    public Door(Vector2Int location, Neighbor neighbor1, Neighbor neighbor2)
    {
        this.neighbor1 = neighbor1;
        this.neighbor2 = neighbor2;
        this.location = location;
    }

    public void open()
    {
        if (opened)
            return;
        opened = true;
        neighbor1.getSector().reveal();
        neighbor2.getSector().reveal();
        Game.getDungeonBoard().board.SetTile((Vector3Int)location, ShiblitzTile.getAppropriateFloorTile(location));
        Game.getCamera().panAndZoomTo(neighbor1.getSector(), neighbor2.getSector());
    }
}
