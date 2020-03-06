using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonSector
{

    public enum SIDE { TOP, RIGHT, BOTTOM, LEFT };
    public enum TYPE { REGULAR, FILLED };

    public Vector2Int position;
    public Vector2Int size;
    public List<Neighbor> neighbors;
    public TYPE type;
    public bool revealed = false;
    public Dungeon dungeon;
    private List<Enemy> enemies;

    public DungeonSector(Vector2Int position, Vector2Int size, Dungeon d, TYPE type)
    {
        dungeon = d;
        enemies = new List<Enemy>();

        this.position = position;
        this.size = size;
        
        setType(type);
    }

    public void setType(TYPE t)
    {
        if(t == TYPE.FILLED)
        {
            type = TYPE.FILLED;
            for(int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    SetTile(new Vector2Int(i, j), ShiblitzTile.TYPE.WALL);
                }
            }
        }
        else
        {
            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    SetTile(new Vector2Int(i, j), ShiblitzTile.TYPE.FLOOR);
                }
            }
        }
    }

    public bool Contains(Vector2Int pointInSectorSpace)
    {
        if (pointInSectorSpace.x > 0 && pointInSectorSpace.x < size.x && pointInSectorSpace.y > 0 && pointInSectorSpace.y < size.y)
            return true;
        return false;
    }

    public void makeCorridors()
    {
        if (type == TYPE.REGULAR)
        {
            Debug.Log("Cant make corridors on a regular room");
            return;
        }
        List<Vector2Int> doors = new List<Vector2Int>();
        for(int i = 0; i < neighbors.Count; i++)
        {
            if(neighbors[i].isConnected())
            {
                Vector2Int nearDoor = dungeonToSectorCoordinates(neighbors[i].door.location);
                SIDE side;
                if (neighbors[i].door.neighbor1.getSector() == this)
                {
                    side = neighbors[i].door.neighbor1.getSide();
                }
                else if (neighbors[i].door.neighbor2.getSector() == this)
                {
                    side = neighbors[i].door.neighbor2.getSide();
                }
                else
                {
                    Debug.Log("corridor error");
                    return;
                }
                if (side == SIDE.TOP)
                    nearDoor.y++;
                else if (side == SIDE.BOTTOM)
                    nearDoor.y--;
                else if (side == SIDE.LEFT)
                    nearDoor.x--;
                else if (side == SIDE.RIGHT)
                    nearDoor.x++;
                doors.Add(nearDoor);
            }
        }
        if (doors.Count > 1)
        {
            doors.Sort((x, y) => x.x - y.x);
            int i = 0;
            Vector2Int marker;
            Vector2Int nextDoor;
            while (i < doors.Count - 1)
            {
                marker = doors[i];
                nextDoor = doors[i + 1];
                SetTile(marker, ShiblitzTile.TYPE.FLOOR);
                while (marker.x < nextDoor.x)
                {
                    marker.x++;
                    SetTile(marker, ShiblitzTile.TYPE.FLOOR);
                }
                while (marker.y < nextDoor.y)
                {
                    marker.y++;
                    SetTile(marker, ShiblitzTile.TYPE.FLOOR);
                }
                while (marker.y > nextDoor.y)
                {
                    marker.y--;
                    SetTile(marker, ShiblitzTile.TYPE.FLOOR);
                }
                i++;
            }
        }
        else if(doors.Count == 1)
        {
            Debug.Log("Reset a corridor room to a regular room");
            setType(TYPE.REGULAR);
        }
    }

    public Neighbor getNeighbor(DungeonSector ds)
    {
        foreach(Neighbor n in neighbors)
        {
            if (n.getSector() == ds)
                return n;
        }
        return null;
    }
    
    public void findNeighbors()
    {
        neighbors = new List<Neighbor>();

        int i = 0;
        // Get neighbors below
        if (position.y > 1)
        {
            while (i < size.x)
            {
                Vector2Int dungeonCoordinates = sectorToDungeonCoordinates(new Vector2Int(i, -2));
                DungeonSector neighbor = dungeon.getSector(dungeonCoordinates);
                if (neighbor != null)
                {
                    neighbors.Add(new Neighbor(neighbor, SIDE.BOTTOM));
                    i = neighbor.position.x + neighbor.size.x + 1 - position.x;
                }
                else
                {
                    i++;
                }
            }
        }
        i = 0;
        // Get neighbors above
        if (position.y + size.y < dungeon.dungeonSize.y - 1)
        {
            while (i < size.x)
            {
                Vector2Int dungeonCoordinates = sectorToDungeonCoordinates(new Vector2Int(i, size.y + 2));
                DungeonSector neighbor = dungeon.getSector(dungeonCoordinates);
                if (neighbor != null)
                {
                    neighbors.Add(new Neighbor(neighbor, SIDE.TOP));
                    i = neighbor.position.x + neighbor.size.x + 1 - position.x;
                }
                else
                {
                    i++;
                }
            }
        }
        i = 0; // left
        if (position.x > 1)
        {
            while (i < size.y)
            {
                Vector2Int dungeonCoordinates = sectorToDungeonCoordinates(new Vector2Int(-2, i));
                DungeonSector neighbor = dungeon.getSector(dungeonCoordinates);
                if (neighbor != null)
                {
                    neighbors.Add(new Neighbor(neighbor, SIDE.LEFT));
                    i = neighbor.position.y + neighbor.size.y + 1 - position.y;
                }
                else
                {
                    i++;
                }
            }
        }
        i = 0; // right
        if (position.x + size.x < dungeon.dungeonSize.x - 1)
        {
            while (i < size.y)
            {
                Vector2Int dungeonCoordinates = sectorToDungeonCoordinates(new Vector2Int(size.x + 2, i));
                DungeonSector neighbor = dungeon.getSector(dungeonCoordinates);
                if (neighbor != null)
                {
                    neighbors.Add(new Neighbor(neighbor, SIDE.RIGHT));
                    i = neighbor.position.y + neighbor.size.y + 1 - position.y;
                }
                else
                {
                    i++;
                }
            }
        }
    }

    public Vector2Int sectorToDungeonCoordinates(Vector2Int sectorCoordinates)
    {
        return new Vector2Int(sectorCoordinates.x + position.x, sectorCoordinates.y + position.y);
    }

    public Vector2Int dungeonToSectorCoordinates(Vector2Int dungeonCoordinates)
    {
        return new Vector2Int(dungeonCoordinates.x - position.x, dungeonCoordinates.y - position.y);
    }

    public void SetTile(Vector2Int position, ShiblitzTile.TYPE type)
    {
        if (position.x < 0 || position.x >= size.x || position.y < 0 || position.y > size.y - 1)
            Debug.Log("tried to set a tile thats outside my bounds!" + position);
        if (dungeon != null)
        {
            dungeon.tiles[this.position.x + position.x][this.position.y + position.y] = new ShiblitzTile(type);
        }
        else
        {
            Debug.Log("FAILED to place tile of type" + type);
        }
    }

    public ShiblitzTile getTile(Vector2Int position)
    {
        return dungeon.tiles[this.position.x + position.x][this.position.y + position.y];
    }

    public void paint(Tilemap target)
    {
        if (revealed)
        {
            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    TileBase type;
                    switch (getTile(new Vector2Int(i,j)).type)
                    {
                        case ShiblitzTile.TYPE.DOOR:
                            type = ShiblitzTile.doorTile;
                            break;
                        case ShiblitzTile.TYPE.FLOOR:
                            type = ShiblitzTile.getAppropriateFloorTile(new Vector2Int(position.x + i, position.y + j));
                            break;
                        case ShiblitzTile.TYPE.WALL:
                            type = ShiblitzTile.wallTile;
                            break;
                        default:
                            Debug.Log("ShiblitzTile Type " + getTile(new Vector2Int(i, j)).type + " is not handled in paint() function");
                            type = ShiblitzTile.bonusTile;
                            break;
                    }
                    target.SetTile(new Vector3Int(position.x + i, position.y + j, 0), type);
                }
            }
        }
        else
        {
            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    target.SetTile(new Vector3Int(position.x + i, position.y + j, 0), ShiblitzTile.wallTile);
                }
            }
        }
    }

    public void reveal()
    {
        if (revealed)
            return;
        foreach(Enemy e in enemies)
        {
            e.reveal();
            e.aggro();
        }
        revealed = true;
        for (int i = 0; i < neighbors.Count; i++)
        {
            if(neighbors[i].isConnected())
            {
                Game.getDungeonBoard().board.SetTile((Vector3Int)neighbors[i].door.location, ShiblitzTile.doorTile);
                if (type == TYPE.FILLED)
                {
                    if (neighbors[i].getSector().type == TYPE.FILLED)
                    {
                        neighbors[i].getSector().reveal();
                        Game.getDungeonBoard().board.SetTile((Vector3Int)neighbors[i].door.location, ShiblitzTile.getAppropriateFloorTile(neighbors[i].door.location));
                    }
                    else
                        Game.getDungeonBoard().board.SetTile((Vector3Int)neighbors[i].door.location, ShiblitzTile.doorTile);
                }
            }
        }
        paint(Game.getDungeonBoard().board);
    }

    public List<Enemy> spawnMonsters()
    {
        if (position.x == 1 && position.y == 1)
            return new List<Enemy>();
        int maxEnemies = 2 + size.x * size.y / 15;
        int number = UnityEngine.Random.Range(0, maxEnemies);
        Debug.Log("Rolled " + number + " enemies (out of " + maxEnemies + " possible) for sector " + position + " (size " + size.x + " x " + size.y + " = " + size.x * size.y + ")");
        if(size.x < 3 || size.y < 3)
        {
            return new List<Enemy>();
        }
        if (number >= (size.x - 1)  * (size.y - 1))
            number = (size.x - 1) * (size.y - 1) -1;
        List<Enemy> enemyList = new List<Enemy>();
        List<Vector2Int> filledSpaces = new List<Vector2Int>();
        for (int i = 0; i < number; i++)
        {
            Vector2Int roomLocation = new Vector2Int(UnityEngine.Random.Range(0, size.x), UnityEngine.Random.Range(0, size.y));
            while (filledSpaces.Contains(roomLocation))
            {
                roomLocation = new Vector2Int(UnityEngine.Random.Range(0, size.x), UnityEngine.Random.Range(0, size.y));
            }
            filledSpaces.Add(roomLocation);
            Vector2Int spawnLocation = sectorToDungeonCoordinates(roomLocation);
            Enemy.TYPE type = Enemy.TYPE.SLIME;
            if(UnityEngine.Random.value > 0.7f)
            {
                type = Enemy.TYPE.SKELLINGTON;
            }
            Enemy e = Game.getEnemyHandler().spawnEnemy(type, spawnLocation);
            enemies.Add(e);
        }
        return enemies;
    }
}

