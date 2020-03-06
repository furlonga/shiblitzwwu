using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Wolf : Enemy
{
    public Wolf(Vector2Int position) : base()
    {
        gameObject = GameObject.Instantiate(werewolf);
        spriteOffset = new Vector2(0.5f, 0.75f);
        setPosition(position);
        moves.Add(new DiagonalOneSquare(this));
    }
}