using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpellManager
{
    List<Spell> activeSpells;

    public SpellManager() {
        activeSpells = new List<Spell>();
    }

    public GameObject castSpell(Spell.Type type, Vector2Int location, List<Vector2Int> areaOfEffect) {
        Spell s = new Spell(type, location, areaOfEffect);
        activeSpells.Add(s);
        return s.cast();
    }

    public void tick() {
        List<Character> charactersInCombat = new List<Character>();
        charactersInCombat.AddRange(Game.getEnemyHandler().getAggroedEnemies());
        charactersInCombat.Add(Game.getPlayer());
        foreach (Spell s in activeSpells) {
            foreach(Character c in charactersInCombat)
            {
                foreach(Vector2Int v in s.areaOfEffect){
                    if(c.position == v)
                        c.takeDamage(s.damage);
                }
            }
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
    public static GameObject fireballImpact = (GameObject) Resources.Load("Spells/FireballImpact");
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
    }

    public GameObject cast() {
        switch(type){
            case Type.FIREBALL:
                foreach(Vector2Int aoePosition in areaOfEffect)
                {
                    Debug.Log(location);
                    Game.getDungeonBoard().spells.SetTile((Vector3Int)aoePosition, ShiblitzMove.fireball);
                }
                duration = 3;
                damage = 3;
                Enemy e = Game.getEnemyHandler().getEnemy(location);
                if(e!= null)
                    e.takeDamage(damage);
                return GameObject.Instantiate(fireballImpact, new Vector3(location.x + 0.5f, location.y + 0.5f, -5), Quaternion.identity);
            case Type.LIGHTNING_STRIKE:
                duration = 1;
                damage = 1;
                break;
            default:
                duration = 1;
                damage = 1;
                break;
        }
        return null;
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