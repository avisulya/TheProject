// Assets/Scripts/Dungeon/Direction.cs
using UnityEngine;

public enum Direction { North, South, East, West }

public static class DirectionUtil
{
    public static Direction Opposite(Direction dir) => dir switch
    {
        Direction.North => Direction.South,
        Direction.South => Direction.North,
        Direction.East  => Direction.West,
        Direction.West  => Direction.East,
        _ => Direction.North
    };

    public static Vector2Int ToGridOffset(Direction dir) => dir switch
    {
        Direction.North => new Vector2Int(0, 1),
        Direction.South => new Vector2Int(0, -1),
        Direction.East  => new Vector2Int(1, 0),
        Direction.West  => new Vector2Int(-1, 0),
        _ => Vector2Int.zero
    };
}