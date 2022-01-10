using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum direction
{
    NORTH = (1 << 0),
    SOUTH = (1 << 1),
    EAST = (1 << 2),
    WEST = (1 << 3)
}

public enum ObstacleRandomness
{
    LOW = 1,
    MID = 3,
    HIGH = 5
}

public enum RoomSize
{
    SMALL = 6,
    MEDIUM = 8,
    BIG = 10,
    HUGE = 12
}