using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiblitzPlayer : Character
{


    /*public void moveUp()
    {
        Dungeon dungeon = Game.getDungeon();
        Vector2Int dungeonPosition = dungeon.getDungeonPosition(transform.position);
        dungeonPosition.y += 1;
        if (dungeon.canMoveTo(dungeonPosition))
            moveTo(dungeonPosition);
    }
    public void moveDown()
    {
        Dungeon dungeon = Game.getDungeon();
        Vector2Int dungeonPosition = dungeon.getDungeonPosition(transform.position);
        dungeonPosition.y -= 1;
        if (dungeon.canMoveTo(dungeonPosition))
            moveTo(dungeonPosition);
    }
    public void moveLeft()
    {
        Dungeon dungeon = Game.getDungeon();
        Vector2Int dungeonPosition = dungeon.getDungeonPosition(transform.position);
        dungeonPosition.x -= 1;
        if (dungeon.canMoveTo(dungeonPosition))
            moveTo(dungeonPosition);
    }
    public void moveRight()
    {
        Dungeon dungeon = Game.getDungeon();
        Vector2Int dungeonPosition = dungeon.getDungeonPosition(transform.position);
        dungeonPosition.x += 1;
        if (dungeon.canMoveTo(dungeonPosition))
            moveTo(dungeonPosition);
    }*/
    public ShiblitzPlayer(GameObject gameObject) : base()
    {
        this.gameObject = gameObject;
        spriteOffset = new Vector2(0.5f, 0.75f);
        health = 10;
        maxHealth = 10;
        maxMana = 10;
        mana = 10;
        speed = 10;
        damage = 2;
    }

    public override void moveTo(Vector2Int position)
    {
        setPosition(position);
        Door d = Game.getDungeon().getDoor(position);
        if (d != null){
            d.open();
        }
    }
    public void setPosition(Vector2Int position)
    {
        this.position = position;
        gameObject.transform.position = new Vector3(position.x, position.y, -2) + (Vector3)spriteOffset;
    }
}
