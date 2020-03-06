using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    protected Character caster;
    protected Vector2Int castLocation;
    protected State state;
    private float timeStarted;
    public int speed = 10;
    public int manaCost = 0;

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

    public bool isFinished()
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

public class ThrowFireball : ShiblitzMove
{
    public ThrowFireball(Character caster) : base(caster)
    {
        manaCost = 5;
    }

    public override List<Vector2Int> getCastableLocations(Vector2Int casterLocation)
    {
        List<Vector2Int> choices = new List<Vector2Int>();
        List<Vector2Int> cardinals = new List<Vector2Int>();
        for (int i = -3; i < 4; i++)
        {
            for (int j = -3; j < 4; j++)
            {
                if (Mathf.Abs(i) + Mathf.Abs(j) <= 3)
                    cardinals.Add(new Vector2Int(i, j));
            }
        }
        foreach (Vector2Int v in cardinals)
        {
            Vector2Int possibleMove = casterLocation + v;
            if (validLocation(possibleMove))
                choices.Add(possibleMove);
        }
        return choices;
    }

    protected override bool validLocation(Vector2Int location)
    {
        Dungeon d = Game.getDungeon();
        if (location.x < 0 || location.x >= d.dungeonSize.x || location.y < 0 || location.y >= d.dungeonSize.y)
            return false;
        return true;
    }
}