using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed
{
    public Vector2Int dungeonSize = new Vector2Int(16, 32);
    public int maxRoomSize = 16;
    public int minRoomSize = 3;
    public int connectivity = 8;
    public float difficulty = 0.5f;
    public float corridorRate = .3f;
}
