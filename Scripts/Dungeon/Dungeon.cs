using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Dungeon
{
    public List<DungeonSector> sectors;
    public List<Door> doors;
    public ShiblitzTile[][] tiles;

    public Vector2Int playerLocation;

    public Vector2Int dungeonSize;
    public int maxRoomSize;
    public int minRoomSize;
    public int connectivity;
    public float difficulty;
    public float corridorRate;

    public Dungeon(Seed seed)
    {
        // Save data from the seed so we dont have to pass it around
        dungeonSize = seed.dungeonSize;
        maxRoomSize = seed.maxRoomSize;
        minRoomSize = seed.minRoomSize;
        connectivity = seed.connectivity;
        difficulty = seed.difficulty;
        corridorRate = seed.corridorRate;

        // Initialize lists
        sectors = new List<DungeonSector>();
        doors = new List<Door>();

        fillDungeonWithWallTiles(); // We're gonna 'carve out' all the floor space 
        sectors = makeSectors(new Vector2Int(1, 1), (dungeonSize - new Vector2Int(2, 2)) ); // Split the space into sectors
        connectSectors(); // Make sure each sector has at least one connection
        connectSectorsAtRandom(connectivity); // Make connectivity number of extra doors
        carveOutCorridors(); // Corridors are full of wall tiles at this point, we need to add floor tiles such that they connect the sectors they share doors with
    }

    public void spawnEnemies(double mean, double deviation)
    {
        List<DungeonSector> tempSectors = sectors.FindAll(delegate(DungeonSector ds)
        {
            return ds.type != DungeonSector.TYPE.FILLED;
        });
            
        //sort the rooms by roomsize and order top down.
        tempSectors.Sort((sector1, sector2)=> sector1.volume.CompareTo(sector2.volume));
        tempSectors.Reverse();

        List<DungeonSector> enemySectors = new List<DungeonSector>();
        List<double> values = new List<double>();

        System.Random r = new System.Random();
        for (int i = 0; i < (int) (tempSectors.Count * .66); i++) 
        {
            double random = NextGaussianDouble(r);
            random *= deviation;
            random += mean;

            values.Add(random);
            enemySectors.Add(tempSectors[i]);
        }

        values.Sort((double1, double2)=> double1.CompareTo(double2));
        enemySectors.Sort((sector1, sector2)=> sector1.distance.CompareTo(sector2.distance));

        for (int i = 0; i < (int) (tempSectors.Count * .66); i++) 
        {
            enemySectors[i].spawnMonsters((int) values[i]);
            //Debug.Log(values[i] + " " + enemySectors[i].distance);
        }

    }

    // Calls makeCorridors() on all filled sectors
    public void carveOutCorridors()
    {
        foreach(DungeonSector sector in sectors)
        {
            if (sector.type == DungeonSector.TYPE.FILLED)
                sector.makeCorridors();
        }
    }

    // Makes connections number of additional connections between sections in the dungeon. Will not connect already-connected sectors, and has a hard cap of 100 to prevent infinite loops
    public void connectSectorsAtRandom(int connections)
    {
        int hardcap = 100;
        while (connections > 0 && hardcap > 0)
        {
            hardcap--;
            connections--;
            int sectorNumber = UnityEngine.Random.Range(0, sectors.Count);
            int neighborNumber = UnityEngine.Random.Range(0, sectors[sectorNumber].neighbors.Count);
            if (sectors[sectorNumber].neighbors[neighborNumber].isConnected())
                connections++;
            else
                connectSectortoNeighbor(sectors[sectorNumber], neighborNumber);
        }
    }

    // Connects sectors until all sectors are connected
    public void connectSectors()
    {
        List<DungeonSector> connectedSectors = new List<DungeonSector>();
        connectedSectors.Add(sectors[0]);
        connectSectorsRecursively(connectedSectors, sectors[0]);
    }

    // Helper function for connectSectors
    public void connectSectorsRecursively(List<DungeonSector> connectedSectors, DungeonSector sector)
    {

        for (int i = 0; i < sector.neighbors.Count; i++)
        {
            if (!connectedSectors.Contains(sector.neighbors[i].getSector()))
            {
                connectSectortoNeighbor(sector, i);
                connectedSectors.Add(sector.neighbors[i].getSector());
                connectSectorsRecursively(connectedSectors, sector.neighbors[i].getSector());
            }
        }
    }

    // Connects a sector to one of its neighbors, chosen by indexing into the neighbor array
    public void connectSectortoNeighbor(DungeonSector a, int neighborIndex)
    {
        int sharedWallCoordinate;
        Vector2Int doorLocation;
        Door door;

        // Get both neighbor objects so that we can attach a reference to the door to each
        Neighbor n = a.neighbors[neighborIndex];
        DungeonSector b = n.getSector();
        Neighbor n2 = b.getNeighbor(a);

        // Decide the location of the door, making sure its on a shared wall between the two sectors
        DungeonSector.SIDE side = n.getSide();
        if (side == DungeonSector.SIDE.BOTTOM || side == DungeonSector.SIDE.TOP)
        {
            sharedWallCoordinate = getRandomSharedWallCoordinate(a.position.x, a.size.x, b.position.x, b.size.x);
            doorLocation = new Vector2Int(sharedWallCoordinate, a.size.y);
            if (side == DungeonSector.SIDE.BOTTOM)
                doorLocation.y = -1;
        }
        else if (side == DungeonSector.SIDE.LEFT || side == DungeonSector.SIDE.RIGHT)
        {
            sharedWallCoordinate = getRandomSharedWallCoordinate(a.position.y, a.size.y, b.position.y, b.size.y);
            doorLocation = new Vector2Int(-1, sharedWallCoordinate);
            if (side == DungeonSector.SIDE.RIGHT)
                doorLocation.x = a.size.x;
        }
        else
            return;

        // Save the door info to the dungeon, as well as the neighbor objects of both sectors
        doorLocation = a.sectorToDungeonCoordinates(doorLocation);
        door = new Door(doorLocation, n, n2);
        if ((a.type == DungeonSector.TYPE.FILLED) && (b.type == DungeonSector.TYPE.FILLED))
            door.opened = true;
        tiles[doorLocation.x][doorLocation.y] = new ShiblitzTile(ShiblitzTile.TYPE.DOOR);
        doors.Add(door);
        n.connect(door);
        n2.connect(door);
    }

    // Helper function that gets a random shared wall x or y coordinate
    public int getRandomSharedWallCoordinate(int sector1Position, int sector1Size, int sector2Position, int sector2Size)
    {

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

        return UnityEngine.Random.Range(doorMin, doorMax) - sector1Position;
    }

    // Fill the entire dungeon with wall tiles. These will be removed as necessary by the DungeonSectors generated
    private void fillDungeonWithWallTiles()
    {
        tiles = new ShiblitzTile[dungeonSize.x][];
        for (int i = 0; i < dungeonSize.x; i++)
        {
            tiles[i] = new ShiblitzTile[dungeonSize.y];
            for (int j = 0; j < dungeonSize.y; j++)
            {
                tiles[i][j] = new ShiblitzTile(ShiblitzTile.TYPE.WALL);
            }
        }
    }

    // Split the allowed space into sectors, then find the neighbors of each section
    private List<DungeonSector> makeSectors(Vector2Int position, Vector2Int size)
    {
        sectors = splitSectorsRecursively(position, size);
        foreach(DungeonSector sector in sectors)
        {
            sector.findNeighbors();
        }
        return sectors;
    }

    // Recursively split the allowed space into sectors
    private List<DungeonSector> splitSectorsRecursively(Vector2Int position, Vector2Int size)
    {
        Vector2Int sector1Position;
        Vector2Int sector2Position;
        Vector2Int sector1Size;
        Vector2Int sector2Size;

        int minimumHorizontalSpace = minRoomSize * 2 + 1; // Need enough space for 2 rooms and a wall tile between them
        int minimumVerticalSpace = minRoomSize * 2 + 1;

        // Build and return the sector if it is too small to split. Also do this randomly sometimes as long as the room is not too big
        if (size.x <= maxRoomSize && size.y <= maxRoomSize && UnityEngine.Random.value > 0.75f || (size.x < minimumHorizontalSpace && size.y < minimumVerticalSpace))
        {
            DungeonSector.TYPE type = DungeonSector.TYPE.REGULAR;
            if (!(position.x == 1 && position.y == 1) && UnityEngine.Random.value > (1 - corridorRate)) // Add a chance to become a corridor as long as we are not the starting room
            {
                type = DungeonSector.TYPE.FILLED;
            }
            
            DungeonSector sector = new DungeonSector(position, size, this, type);
            return new List<DungeonSector> { sector };
        }

        bool verticalSplitAllowed = size.x >= minimumHorizontalSpace;
        bool horizontalSplitAllowed = size.y >= minimumVerticalSpace;

        // Pick between veritcal and horizontal split
        if (horizontalSplitAllowed && verticalSplitAllowed)
        {
            if (UnityEngine.Random.value > 0.5f)
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
            int allowedVariance = (size.x - minimumHorizontalSpace) / 2;
            int extra = UnityEngine.Random.Range(0, allowedVariance + 1);
            if (allowedVariance > 0 && extra == 0)
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
            sector1Position = position;
            sector1Size = new Vector2Int((size.x / 2) + extra, size.y);
            sector2Size = new Vector2Int(size.x - sector1Size.x - 1, size.y);
            sector2Position = new Vector2Int(position.x + sector1Size.x + 1, position.y);
        }

        else if (horizontalSplitAllowed) // Do a horizontal split
        {
            int allowedVariance = (size.y - minimumVerticalSpace) / 2;
            int extra = UnityEngine.Random.Range(0, allowedVariance + 1);
            if (allowedVariance > 0 && extra == 0)
            {
                extra = allowedVariance;
            }
            if (UnityEngine.Random.value > 0.5f)
            {
                extra *= -1;
            }
            sector1Position = position;
            sector1Size = new Vector2Int(size.x, (size.y / 2) + extra);
            sector2Size = new Vector2Int(size.x, size.y - sector1Size.y - 1);
            sector2Position = new Vector2Int(position.x, position.y + sector1Size.y + 1);
        }
        else
        {
            Debug.Log("neither horizontal or vertical split was allowed, but sector was split anyways!");
            return null;
        }

        List<DungeonSector> firstSectorList = splitSectorsRecursively(sector1Position, sector1Size);
        List<DungeonSector> secondSectorList = splitSectorsRecursively(sector2Position, sector2Size);
        firstSectorList.AddRange(secondSectorList);
        return firstSectorList;
    }

    // Gets the sector that contains a given point in the dungeon
    public DungeonSector getSector(Vector2Int position)
    {
        foreach (DungeonSector d in sectors)
        {
            if (position.x >= d.position.x && position.x < d.position.x + d.size.x &&
                position.y >= d.position.y && position.y < d.position.y + d.size.y)
            {
                return d;
            }
        }
        return null;
    }

    // Returns true if the given location is occupiable
    public bool canMoveTo(Vector2Int location)
    {
        DungeonSector d = getSector(location);
        if(d != null)
        {
            return tiles[location.x][location.y].type == ShiblitzTile.TYPE.FLOOR;
        }
        else
        {
            if (getDoor(location) != null)
                return true;
        }
        return false;
    }

    public Door getDoor(Vector2Int location)
    {
        foreach(Door door in doors)
        {
            if (door.location == location)
                return door;
        }
        return null;
    }

    public Vector2Int getDungeonPosition(Vector3 position)
    {
        return new Vector2Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
    }

    // Checks if a move by the player was legal. Also reveals rooms if the player opened a door
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
                    door.open();
                    return true;
                }
            }
        }
        else
        {
            if(tiles[dungeonPosition.x][dungeonPosition.y].type == ShiblitzTile.TYPE.WALL)
            {
                return false;
            }
            playerLocation = dungeonPosition;
            return true;
        }
        return false;
    }

    // Show where the player can move this turn (only up, down, left, right for now)
    public void showBasicMovementLocations()
    {

    }
}
