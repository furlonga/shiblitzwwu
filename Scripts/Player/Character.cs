using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character
{
    public Vector2 spriteOffset = new Vector2(0.5f, 0.75f);
    public Vector2Int position;
    public GameObject gameObject;

    public int health = 5;
    public int maxHealth = 5;
    public int mana = 5;
    public int maxMana = 5;
    public int speed = 5;
    public int damage = 1;

    public abstract void moveTo(Vector2Int position);

    public void gainMana(int amount) {
        mana += amount;
        if(mana > maxMana)
            mana = maxMana;
    }

    public void gainHealth(int amount) {
        health += amount;
        if(health > maxHealth)
            health = maxHealth;
    }

    public virtual bool takeDamage(int damage) {
        Debug.Log("someone took damage!");
        health -= damage;
        if(this is Enemy){
            if(health <= 0){
                Game.getEnemyHandler().enemies.Remove((Enemy)this);
                Game.getEnemyHandler().getAggroedEnemies().Remove((Enemy)this);
                GameObject.Destroy(gameObject);
                return true;
            }
        }
        else if(this is ShiblitzPlayer){
            Game.getUIManager().reflectPlayerStats();
            if(health <=0){
                Debug.Log("Player died!!");
                GameObject.Destroy(gameObject);
            }
        }
        return false;
    }
}