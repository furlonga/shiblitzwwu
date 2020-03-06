using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : Character
{
    public static GameObject slime = (GameObject)Resources.Load("Enemies/Slime");
    public static GameObject werewolf = (GameObject)Resources.Load("Enemies/Werewolf");
    public static GameObject skellington = (GameObject)Resources.Load("Enemies/Skellington");
    
    public bool aggroed = false;
    
    public List<ShiblitzMove> moves = new List<ShiblitzMove>();

    public enum TYPE { SLIME, WEREWOLF, SKELLINGTON }

    public Enemy()
    {

    }

    public void queueMove()
    {
        ShiblitzMove move = moves[UnityEngine.Random.Range(0, moves.Count)];
        List<Vector2Int> possibleMoves = move.getCastableLocations(position);
        if (possibleMoves.Count > 0)
        {
            Vector2Int moveLocation = possibleMoves[0];
            foreach(Vector2Int v in possibleMoves)
            {
                if (Vector2Int.Distance(v, Game.getPlayer().position) < Vector2Int.Distance(moveLocation, Game.getPlayer().position))
                    moveLocation = v;
            }
            Game.getDungeonBoard().gui.SetTile((Vector3Int)moveLocation, ShiblitzTile.enemyInputHighlight);
            move.setCastLocation(moveLocation);
            Game.QueueMove(move);
        }
    }

    public override void moveTo(Vector2Int position)
    {
        setPosition(position);
        Game.getDungeonBoard().gui.SetTile((Vector3Int)position, null);
    }

    public void setPosition(Vector2Int position)
    {
        this.position = position;
        this.gameObject.transform.position = new Vector3(position.x, position.y, -2) + (Vector3)spriteOffset;
    }

    public void reveal()
    {
        Color c = gameObject.GetComponent<SpriteRenderer>().color;
        c.a = 1;
        gameObject.GetComponent<SpriteRenderer>().color = c;
    }

    public void aggro()
    {
        aggroed = true;
    }
}