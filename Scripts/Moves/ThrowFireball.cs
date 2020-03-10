using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowFireball : SpellMove
{

    GameObject impact;
    public ThrowFireball(Character caster) : base(caster)
    {
        manaCost = 5;
        speed = 25;
    }

    public override void performMove() {
        base.performMove();
        List<Vector2Int> aoe = new List<Vector2Int>();
        List<Vector2Int> cardinals = new List<Vector2Int>(){ new Vector2Int(-1, 0),new Vector2Int(0, -1),new Vector2Int(0, 1),new Vector2Int(1, 0),new Vector2Int(0, 0) };
        foreach (Vector2Int v in cardinals) {
            
            if(validLocation(v + castLocation)) {
                aoe.Add(new Vector2Int(v.x + castLocation.x, v.y + castLocation.y));
            }

        }
        impact = Game.getSpellManager().castSpell(Spell.Type.FIREBALL, castLocation,  aoe);
    }

    public override bool isFinished(){
        return impact == null;
    }
}