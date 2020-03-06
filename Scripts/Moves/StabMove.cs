using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabMove : AttackingMove
{
    public StabMove(Character c) : base(c) { }

    public override List<Vector2Int> getCastableLocations(Vector2Int casterLocation)
    {

        List<Vector2Int> choices = new List<Vector2Int>();
        List<Vector2Int> cardinals = new List<Vector2Int> { new Vector2Int(-1, -1), new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, 1), new Vector2Int(0, 1), new Vector2Int(0, -1), new Vector2Int(-1, 0), new Vector2Int(1, 0) };
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
        AffectedTiles = new List<Vector2Int>();
        Vector2Int difference = castLocation - caster.position;
        difference = castLocation + difference;
        AffectedTiles.Add(castLocation);
        AffectedTiles.Add(difference);
        base.performMove();

        foreach (Vector2Int vec in AffectedTiles) {
            Enemy enemy = Game.getEnemyHandler().getEnemy(vec);
            if (enemy != null) {
                enemy.health = enemy.health - caster.damage;
            }
        }
    }
}