using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodMove : LocationChangingMove
{
    public GodMove(Character caster) : base(caster) { speed = 15; manaCost = 3; }

    public override List<Vector2Int> getCastableLocations(Vector2Int casterLocation)
    {
        List<Vector2Int> choices = new List<Vector2Int>();
        List<Vector2Int> cardinals = new List<Vector2Int>();
        for(int i = -3; i < 4; i++)
        {
            for (int j = -3; j < 4; j++)
            {
                if(Mathf.Abs(i) + Mathf.Abs(j) <= 3)
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

    public override void performMove()
    {
        base.performMove();
        caster.moveTo(castLocation);
    }
}
