using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler
{
    public List<Enemy> enemies;

    public EnemyHandler()
    {
        enemies = new List<Enemy>();
    }

    public Enemy getEnemy(Vector2Int location)
    {
        foreach(Enemy enemy in enemies)
        {
            if (enemy.position == location)
                return enemy;
        }
        return null;
    }

    public Enemy spawnEnemy(Enemy.TYPE type, Vector2Int location)
    {
        //GameObject enemyGO = GameObject.Instantiate(slime, new Vector3Int(location.x, location.y, -3), Quaternion.identity);
        Enemy enemy;
        switch (type)
        {
            case Enemy.TYPE.SLIME:
                enemy = new Slime(location);
                break;
            case Enemy.TYPE.SKELLINGTON:
                enemy = new Skellington(location);
                break;
                case Enemy.TYPE.WEREWOLF:
                enemy = new Wolf(location);
                break;
            default:
                enemy = new Slime(location);
                break;
        }
        enemies.Add(enemy);
        return enemy;
    }

    public List<Enemy> getAggroedEnemies()
    {
        List<Enemy> aggroed = new List<Enemy>();
        foreach(Enemy e in enemies)
        {
            if (e.aggroed)
                aggroed.Add(e);
        }
        return aggroed;
    }
}
