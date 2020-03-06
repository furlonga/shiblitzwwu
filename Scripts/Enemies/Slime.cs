using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    public Slime(Vector2Int position) : base()
    {
        gameObject = GameObject.Instantiate(slime);
        spriteOffset = new Vector2(0.5f, 0.7f);
        setPosition(position);
        moves.Add(new CardinalOneSquare(this));
    }
}