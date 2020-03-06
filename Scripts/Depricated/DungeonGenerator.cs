using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/*public class DungeonGenerator : MonoBehaviour
{
    public ShiblitzPlayer player;

    // Runtime variables
    public Vector2Int dungeonSize = new Vector2Int(16, 32);
    public int maxRoomSize = 16;
    public int minRoomSize = 3;
    public int connectivity = 8;
    public float difficulty = 0.5f;
    public float corridorRate = 0.3f;

    // Exposed variables to link objects without having to use code -- Change this
    public Tilemap background;
    public Tilemap foreground;

    private DungeonSector dungeon;
    private List<DungeonSector> sectors;

    private List<Door> doors;
    private List<DungeonSector> connectedSectors;

    private List<Enemy> enemies;
    private Vector2Int playerLocation;

    // Start is called before the first frame update
    void Start()
    {
        player.setPosition(new Vector2(1,1));
        playerLocation = new Vector2Int(1, 1);
        enemies = new List<Enemy>();
        doors = new List<Door>();

        CameraController c = Camera.main.GetComponent<CameraController>();
        c.zoomMax = Math.Max(dungeonSize.x, dungeonSize.y * 0.55f);
        c.zoomOutAndCenter(new Vector2(dungeonSize.x / 2, dungeonSize.y / 2));


        connectedSectors = new List<DungeonSector>();

        if(corridorRate > 0.9f)
        {
            corridorRate = 0.9f;
        }

        // Move and scale the camera depending on the board size
        //Camera.main.transform.position = new Vector3(dungeonSize.x / 2, dungeonSize.y / 2, -10);
        //Camera.main.orthographicSize = Math.Max(dungeonSize.x, dungeonSize.y) * 0.55f;

        // Start making rooms        
        for (int i = 0; i < dungeonSize.x; i++)
        {
            for (int j = 0; j < dungeonSize.y; j++)
            {
                foreground.SetTile( (Vector3Int) new Vector2Int(i, j), ShiblitzTile.wallTile);
            }
        }
        dungeon = new DungeonSector(new Vector2Int(1, 1), dungeonSize - new Vector2Int(2, 2));
        sectors = breakDungeonIntoSectors(dungeon);
        for(int i = 0; i < sectors.Count; i++)
        {
            sectors[i].setNeighbors(this);
        }
        connectedSectors.Add(sectors[sectors.Count/2]);
        connectSectorsRecursively(ref connectedSectors, sectors[sectors.Count/2]);

        connectSectorsAtRandom(connectivity);

        for(int i = 0; i < sectors.Count; i++)
        {
            if(sectors[i].type == DungeonSector.TYPE.FILLED)
            {
                sectors[i].makeCorridors();
            }
            sectors[i].paint(foreground);
        }
        getSector(new Vector2Int(1, 1)).reveal(foreground);
    }
    
    public static Dungeon generateDungeon(Seed seed)
    {
        Dungeon dungeon = new Dungeon(seed);


        return dungeon;
    }

    public void connectSectorsAtRandom(int connections)
    {
        int hardcap = 100;
        while(connections > 0 && hardcap > 0)
        {
            hardcap--;
            connections--;
            int sectorNumber = UnityEngine.Random.Range(0, sectors.Count);
            int neighborNumber = UnityEngine.Random.Range(0, sectors[sectorNumber].neighbors.Count);
            if (sectors[sectorNumber].neighbors[neighborNumber].isConnected())
                connections++;
            else
                connectSectors(sectors[sectorNumber], neighborNumber);
        }
    }

    public void connectSectorsRecursively(ref List<DungeonSector> connected, DungeonSector sector)
    {
        for(int i = 0; i < sector.neighbors.Count; i++)
        {
            if(!connected.Contains(sector.neighbors[i].getSector())) {
                connectSectors(sector, i);
                //sector.neighbors[i].getSector().paintDebug(foreground);
                connected.Add(sector.neighbors[i].getSector());
                connectSectorsRecursively(ref connected, sector.neighbors[i].getSector());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get the mouse position in world coordinates
            Vector3 mousePositionInGridCoordinates = foreground.WorldToCell(mousePosition); // Convert coordinates to cell space
            Vector2Int tilePosition = new Vector2Int(Mathf.FloorToInt(mousePositionInGridCoordinates.x), Mathf.FloorToInt(mousePositionInGridCoordinates.y)); // Make a Vector2Int to use as cell indeces
            DungeonSector clickedSector = getSector(tilePosition);
            if(clickedSector != null)
            {
                clickedSector.paintDebug(foreground);
                for(int i = 0; i < clickedSector.neighbors.Count; i++)
                {
                    if(clickedSector.neighbors[i].isConnected())
                        clickedSector.neighbors[i].getSector().paintDebug(foreground);
                }
            }
            
        }
        if (Input.GetMouseButtonUp(0))
        {
            foreach(DungeonSector s in sectors)
            {
                s.paint(foreground);
            }
        }
        for(int i = 0; i < enemies.Count; i++)
        {
            if(enemies[i].position == playerLocation)
            {
                player.setPosition(new Vector2Int(1, 1));
                playerLocation = new Vector2Int(1, 1);
            }
        }
    }

    public void connectSectors(DungeonSector a, int neighborIndex)
    {
        if (neighborIndex >= a.neighbors.Count)
        {
            Debug.Log("ERROR: Neighbor index out of bounds!");
            return;
        }
        Neighbor n = a.neighbors[neighborIndex];
        DungeonSector b = n.getSector();

        Vector2Int doorLocation;
        int door;
        Door d;
        Neighbor n2 = b.getNeighbor(a);
        if (n2 == null)
        {
            Debug.Log("Neighbor connection only went one way");
            return;
        }

        DungeonSector.SIDE side = n.getSide();
        if(side == DungeonSector.SIDE.BOTTOM || side == DungeonSector.SIDE.TOP)
        {
            door = getDoorLocation(a.position.x, a.size.x, b.position.x, b.size.x);
            if (door < 0)
            {
                Debug.Log("Sectors listed as neighbors couldn't find a shared wall");
                return;
            }
            doorLocation = new Vector2Int(door, a.size.y);
            if (side == DungeonSector.SIDE.BOTTOM)
                doorLocation.y = -1;
        }
        else if(side == DungeonSector.SIDE.LEFT || side == DungeonSector.SIDE.RIGHT)
        {
            door = getDoorLocation(a.position.y, a.size.y, b.position.y, b.size.y);
            if (door < 0) {
                Debug.Log("Sectors listed as neighbors couldn't find a shared wall");
                return;
            }
            doorLocation = new Vector2Int(-1, door);
            if (side == DungeonSector.SIDE.RIGHT)
                doorLocation.x = a.size.x;
        }
        else
        {
            Debug.Log("Neighbor object did not have a valid Side value IN connectSectors. Side: " + n.getSide());
            return;
        }
        doorLocation = a.sectorToDungeonCoordinates(doorLocation);
        d = new Door(doorLocation, n, n2);
        doors.Add(d);
        n.connect(d);
        n2.connect(d);
    }

    public int getDoorLocation(int sector1Position, int sector1Size,  int sector2Position, int sector2Size) {

        int sector1Min = sector1Position;
        if (sector1Min == 0) // If we're on the far left side, dont consider the leftmost tile
            sector1Min = 1;
        int sector2Min = sector2Position;
        if (sector2Min == 0)
            sector2Min = 1;

        int sector1Max = sector1Position + sector1Size; // -2 to account for the wall on the righthand side
        int sector2Max = sector2Position + sector2Size;

        int doorMin = Math.Max(sector1Min, sector2Min);
        int doorMax = Math.Min(sector1Max, sector2Max);

        if (doorMax - doorMin < 0)
        {
            Debug.Log("Door range less than 0");
            return -1;
        }
        
        return UnityEngine.Random.Range(doorMin, doorMax) - sector1Position;
    }

    public DungeonSector getSector(Vector2Int position)
    {
        foreach (DungeonSector d in sectors) {
            if(position.x >= d.position.x && position.x < d.position.x + d.size.x &&
                position.y >= d.position.y && position.y < d.position.y + d.size.y)
            {
                return d;
            }
        }
        return null;
    }

    private List<DungeonSector> breakDungeonIntoSectors(DungeonSector d)
    {
        DungeonSector sector1;
        DungeonSector sector2;
        Vector2Int sector1Position;
        Vector2Int sector2Position;
        Vector2Int sector1Size;
        Vector2Int sector2Size;

        int minimumHorizontalSpace = minRoomSize * 2 + 2;
        if (d.position.x == 0)
        {
            minimumHorizontalSpace++;
        }
        int minimumVerticalSpace = minRoomSize * 2 + 2;
        if (d.position.y == 0)
        {
            minimumVerticalSpace++;
        }

        // If the current sector is not bigger than the maximum room size, add a chance of not splitting
        if ( d.size.x <= maxRoomSize && d.size.y <= maxRoomSize && UnityEngine.Random.value > 0.75f || (d.size.x < minimumHorizontalSpace && d.size.y < minimumVerticalSpace))
        {
            if(d.size.x < minRoomSize || d.size.y < minRoomSize)
            {
                Debug.Log("We made a room too small!!");
            }
            if(!(d.position.x == 1 && d.position.y == 1) && UnityEngine.Random.value > (1-corridorRate))
            {
                d.setType(DungeonSector.TYPE.FILLED);
            }
            else
            {
                //d.setType(DungeonSector.TYPE.REGULAR);
                if(UnityEngine.Random.value > (1 - difficulty) && !(d.position.x == 1 && d.position.y == 1))
                {
                    enemies.AddRange(d.spawnMonsters());
                }
            }
            return new List<DungeonSector> { d };
        }

        bool verticalSplitAllowed = d.size.x >= minimumHorizontalSpace;
        bool horizontalSplitAllowed = d.size.y >= minimumVerticalSpace;

        // Pick between veritcal and horizontal split
        if(horizontalSplitAllowed && verticalSplitAllowed)
        {
            if(UnityEngine.Random.value > 0.5f)
            {
                horizontalSplitAllowed = false;
            }
            else
            {
                verticalSplitAllowed = false;
            }
        }
        if (verticalSplitAllowed) // Do a vertical split
        {
            int allowedVariance = (d.size.x - minimumHorizontalSpace) / 2;
            int extra = UnityEngine.Random.Range(0, allowedVariance + 1);
            if(allowedVariance > 0 && extra == 0)
            {
                extra = allowedVariance;
            }
            if (allowedVariance == 0)
            {
                extra = 0;
            }
            if (UnityEngine.Random.value > 0.5f)
            {
                extra *= -1;
            }
            if(d.position.x == 0)
            {
                extra++;
            }
            sector1Position = d.position;
            sector1Size = new Vector2Int((d.size.x / 2) + extra - 1, d.size.y);
            sector2Size = new Vector2Int(d.size.x - sector1Size.x - 1, d.size.y);
            sector2Position = new Vector2Int(d.position.x + sector1Size.x + 1, d.position.y);

            sector1 = new DungeonSector(sector1Position, sector1Size);
            sector2 = new DungeonSector(sector2Position, sector2Size);
        }

        else if(horizontalSplitAllowed) // Do a horizontal split
        {
            int allowedVariance = (d.size.y - minimumVerticalSpace) / 2;
            int extra = UnityEngine.Random.Range(0, allowedVariance + 1);
            if (allowedVariance > 0 && extra == 0)
            {
                extra = allowedVariance;
            }
            if(allowedVariance == 0)
            {
                extra = 0;
            }
            if (UnityEngine.Random.value > 0.5f)
            {
                extra *= -1;
            }
            if (d.position.y == 0)
            {
                extra++;
            }
            sector1Position = d.position;
            sector1Size = new Vector2Int(d.size.x, (d.size.y / 2) + extra - 1);
            sector2Size = new Vector2Int(d.size.x, d.size.y - sector1Size.y - 1);
            sector2Position = new Vector2Int(d.position.x, d.position.y + sector1Size.y + 1);

            sector1 = new DungeonSector(sector1Position, sector1Size);
            sector2 = new DungeonSector(sector2Position, sector2Size);
        }
        else
        {
            Debug.Log("neither horizontal or vertical split was allowed, but sector was split anyways!");
            return null;
        }
            
        List<DungeonSector> firstSectorList = breakDungeonIntoSectors(sector1);
        List<DungeonSector> secondSectorList = breakDungeonIntoSectors(sector2);
        firstSectorList.AddRange(secondSectorList);
        return firstSectorList;
    }

    public bool movePlayerTo(Vector2 newPosition)
    {
        Vector2Int dungeonPosition = new Vector2Int(Mathf.FloorToInt(newPosition.x), Mathf.FloorToInt(newPosition.y));
        DungeonSector d = getSector(dungeonPosition);
        if (d == null)
        {
            foreach (Door door in doors)
            {
                if (door.location == dungeonPosition)
                {
                    door.neighbor1.getSector().reveal(foreground);
                    door.neighbor2.getSector().reveal(foreground);
                    foreground.SetTile((Vector3Int)dungeonPosition, ShiblitzTile.floorDarkTile);
                    return true;
                }
            }
        }
        else
        {
            if (d.getTile(d.dungeonToSectorCoordinates(dungeonPosition)).type == ShiblitzTile.TYPE.WALL)
            {
                Debug.Log("tried to move onto wall");
                return false;
            }
            else if (d.getTile(d.dungeonToSectorCoordinates(dungeonPosition)).type == ShiblitzTile.TYPE.DOOR)
            {
                DungeonSector ds = getSector(dungeonPosition);
                for (int i = 0; i < sectors.Count; i++)
                {
                    for (int j = 0; j < sectors[i].neighbors.Count; j++)
                    {
                        if (sectors[i].neighbors[j].ownsDoor(sectors[i].dungeonToSectorCoordinates(dungeonPosition)))
                        {
                            sectors[i].reveal(foreground);
                            sectors[i].neighbors[j].getSector().reveal(foreground);
                        }
                    }
                }
                ds.SetTile(ds.dungeonToSectorCoordinates(dungeonPosition), ShiblitzTile.TYPE.FLOOR);
                ds.reveal(foreground);
            }
            playerLocation = dungeonPosition;
            return true;
        }
        return false;
    }
}*/