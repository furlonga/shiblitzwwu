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
}
