using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager
{
    List<Spell> activeSpells;

    public SpellManager() {
        activeSpells = new List<Spell>();
    }

    public void castSpell(Spell.Type type, Vector2Int location, List<Vector2Int> areaOfEffect) {
        Spell s = new Spell(type, location, areaOfEffect);
        activeSpells.Add(s);
    }

    public void tick() {
        foreach (Spell s in activeSpells) {
            s.tick();
        }
        for(int i = 0; i < activeSpells.Count; i++) {
            if(activeSpells[i].duration <= 0){
                Game.getDungeonBoard().spells.SetTile((Vector3Int) activeSpells[i].location, null);
                activeSpells[i].clean();
                activeSpells.Remove(activeSpells[i]);
                i--;
            }
        }
    }

    public Spell getSpell(Vector2Int location) {
        foreach(Spell s in activeSpells){
            if(s.location == location)
                return s;
        }
        return null;
    }
}

public class Spell {
    public enum Type { FIREBALL, LIGHTNING_STRIKE }
    public Type type;
    public List<Vector2Int> areaOfEffect;
    public Vector2Int location;
    public int duration;
    public int damage;
    
    public Spell(Type type, Vector2Int location, List<Vector2Int> areaOfEffect) {
        this.type = type;
        this.location = location;
        this.areaOfEffect = areaOfEffect;
        switch(type){
            case Type.FIREBALL:
                foreach(Vector2Int aoePosition in areaOfEffect)
                {
                    Game.getDungeonBoard().spells.SetTile((Vector3Int)location, ShiblitzMove.fireball);
                }
                duration = 3;
                damage = 3;
                break;
            case Type.LIGHTNING_STRIKE:
                duration = 1;
                damage = 1;
                break;
            default:
                duration = 1;
                damage = 1;
                break;
        }
    }

    public void tick() {
        duration--;
    }

    public void clean() {
        foreach(Vector2Int location in areaOfEffect){
                Game.getDungeonBoard().spells.SetTile((Vector3Int) location, null);
        }
    }

}