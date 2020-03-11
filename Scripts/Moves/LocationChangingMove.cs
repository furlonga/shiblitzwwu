using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LocationChangingMove : ShiblitzMove
{
    public LocationChangingMove(Character caster) :base(caster) { }
    public override void setCastLocation(Vector2Int location )
    {
        base.setCastLocation(location);
        Game.getDungeonBoard().occupiedSpaces.Add(location);
    }

    protected override bool validLocation(Vector2Int location)
    {
        Dungeon dungeon = Game.getDungeon();
        // return false if location is out of bounds
        if (location.x < 0 || location.x >= dungeon.dungeonSize.x)
            return false;
        if (location.y < 0 || location.y >= dungeon.dungeonSize.y)
            return false;
        // return false if location is on a wall
        if (dungeon.tiles[location.x][location.y].type == ShiblitzTile.TYPE.WALL)
            return false;
        // If the caster is an enemy, dont let them cast on closed doors
        Door d = dungeon.getDoor(location);
        if (caster is Enemy)
        {
            if (Game.getDungeonBoard().occupiedSpaces.Contains(location))
                return false;
            if (d != null && !d.opened)
                return false;
        }
        else
        {
            if (d != null && !d.neighbor1.getSector().revealed && !d.neighbor2.getSector().revealed)
                return false;
        }
        // If the location is in an unrevealed sector, return false
        DungeonSector s = dungeon.getSector(location);
        if (d == null && (s == null || !s.revealed))
            return false;
        return true;
    }
}
