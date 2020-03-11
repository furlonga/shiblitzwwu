using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighbor
{
    private DungeonSector sector;
    private DungeonSector.SIDE side;
    private bool connected;
    public Door door;

    public Neighbor(DungeonSector sector, DungeonSector.SIDE side)
    {
        this.sector = sector;
        this.side = side;

        connected = false;
    }

    public bool equals(DungeonSector d)
    {
        if (d == sector)
        {
            return true;
        }
        return false;
    }

    public DungeonSector getSector()
    {
        return sector;
    }

    public DungeonSector.SIDE getSide()
    {
        return side;
    }

    public bool isConnected()
    {
        return connected;
    }

    public void connect(Door d)
    {
        connected = true;
        door = d;
    }

    public bool ownsDoor(Vector2Int location)
    {
        if(door.location == location)
        {
            return true;
        }
        return false;
    }
}
