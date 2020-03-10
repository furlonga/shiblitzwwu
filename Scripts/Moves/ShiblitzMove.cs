using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
 * Need to be able to:
 * Calculate possible locations for player to choose
 * Show GUI for these locations
 * Have the user pick one
 * Clean up the GUI tiles that we laid out
 * perform the move 
 */
public abstract class ShiblitzMove
{
    public static TileBase fireball = (TileBase)Resources.Load("Spells/Fireball");
    protected Character caster;
    protected Vector2Int castLocation;
    protected State state;
    protected float timeStarted;
    public int speed = 10;
    public int manaCost = 0;
    public bool isCantrip = false;

    public enum State { READY, IN_PROGRESS, FINISHED}

    public ShiblitzMove(Character caster)
    {
        this.caster = caster;
        state = State.READY;
    }
    // Should return a list of all possible valid locations to cast the given move when cast from casterLocation
    public abstract List<Vector2Int> getCastableLocations(Vector2Int casterLocation);
    public virtual void performMove()
    {
        state = State.IN_PROGRESS;
        timeStarted = Time.time;
        caster.mana -= manaCost;
    }

    public virtual bool isFinished()
    {
        return Time.time - timeStarted > .15f;
    }

    public bool started()
    {
        return state == State.IN_PROGRESS || state == State.FINISHED;
    }

    public virtual void setCastLocation(Vector2Int location)
    {
        castLocation = location;
        state = State.READY;
    }

    protected virtual bool validLocation(Vector2Int location)
    {
        return true;
    }
}