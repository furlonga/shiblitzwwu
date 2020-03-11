using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMove : ShiblitzMove
{

    public SpellMove(Character caster) : base(caster) { }
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
