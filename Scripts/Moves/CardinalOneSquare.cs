using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardinalOneSquare : LocationChangingMove
{
    public CardinalOneSquare(Character caster) : base(caster) { speed = 8; }

    public override List<Vector2Int> getCastableLocations(Vector2Int casterLocation)
    {

        List<Vector2Int> choices = new List<Vector2Int>();
        List<Vector2Int> cardinals = new List<Vector2Int> { new Vector2Int(-1, 0), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(0, 1) };
        foreach (Vector2Int v in cardinals)
        {
            Vector2Int possibleMove = casterLocation + v;
            if (validLocation(possibleMove))
                choices.Add(possibleMove);
        }
        return choices;
    }


    public override void performMove()
    {
        base.performMove();
        caster.moveTo(castLocation);
    }
}