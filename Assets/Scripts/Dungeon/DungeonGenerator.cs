// Assets/Scripts/Dungeon/DungeonGenerator.cs
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator
{
    private Dictionary<Vector2Int, bool>            _occupied = new();
    private Dictionary<RoomType, List<Direction>>    _roomDoors;

    public static DungeonNode Generate(int criticalLength, float branchChance,
                                       Dictionary<RoomType, List<Direction>> availableDoors)
    {
        return new DungeonGenerator()._Generate(criticalLength, branchChance, availableDoors);
    }

    private DungeonNode _Generate(int criticalLength, float branchChance,
                                  Dictionary<RoomType, List<Direction>> availableDoors)
    {
        _roomDoors = availableDoors;
        _occupied.Clear();

        var root = new DungeonNode
        {
            roomType     = RoomType.Entry,
            gridPosition = Vector2Int.zero
        };
        _occupied[Vector2Int.zero] = true;

        var current  = root;
        var lastDir  = (Direction?)null;

        for (int i = 1; i < criticalLength; i++)
        {
            bool isLast = i == criticalLength - 1;
            var nextType = isLast ? RoomType.Boss : RoomType.Combat;

            Direction? chosenDir;
            if (isLast)
            {
                var north    = Direction.North;
                var nextPos  = current.gridPosition + DirectionUtil.ToGridOffset(north);
                if (!_occupied.ContainsKey(nextPos) && CanGo(current.roomType, north, nextType))
                    chosenDir = north;
                else
                    return null;   // fail, caller retries
            }
            else
            {
                chosenDir = PickFreeDirection(current.roomType, nextType,
                                              current.gridPosition, lastDir);
            }

            if (chosenDir == null) return null;

            var nextPos2 = current.gridPosition + DirectionUtil.ToGridOffset(chosenDir.Value);
            var mainNode = new DungeonNode
            {
                roomType            = nextType,
                gridPosition        = nextPos2,
                parentExitDirection = chosenDir.Value,
                parent              = current
            };
            current.mainChild = mainNode;
            _occupied[nextPos2] = true;

            // Optional branch
            if (!isLast && Random.value < branchChance)
            {
                var sideType = Random.value < 0.5f ? RoomType.Treasure : RoomType.Shop;
                var sideDir  = PickFreeDirection(current.roomType, sideType,
                                                 current.gridPosition, lastDir, nextPos2);
                if (sideDir != null)
                {
                    var sidePos = current.gridPosition + DirectionUtil.ToGridOffset(sideDir.Value);
                    var branch  = new DungeonNode
                    {
                        roomType            = sideType,
                        gridPosition        = sidePos,
                        parentExitDirection = sideDir.Value,
                        parent              = current
                    };
                    current.branchChild = branch;
                    _occupied[sidePos] = true;
                }
            }

            lastDir = DirectionUtil.Opposite(chosenDir.Value);
            current = mainNode;
        }

        return root;
    }

    private Direction? PickFreeDirection(RoomType parentType, RoomType childType,
        Vector2Int from, Direction? avoidDir,
        Vector2Int? excludePos = null)
    {
        var parentDirs = _roomDoors.GetValueOrDefault(parentType, new List<Direction>());
        var childDirs  = _roomDoors.GetValueOrDefault(childType,  new List<Direction>());
        var candidates = new List<Direction>();

        foreach (var dir in parentDirs)
        {
            if (!childDirs.Contains(DirectionUtil.Opposite(dir))) continue;
            if (avoidDir != null && dir == avoidDir.Value) continue;

            var next = from + DirectionUtil.ToGridOffset(dir);
            if (_occupied.ContainsKey(next)) continue;
            if (excludePos != null && next == excludePos.Value) continue;

            candidates.Add(dir);
        }

        if (candidates.Count == 0) return null;
        return candidates[Random.Range(0, candidates.Count)];
    }

    private bool CanGo(RoomType parentType, Direction dir, RoomType childType)
    {
        var parentDirs = _roomDoors.GetValueOrDefault(parentType, new List<Direction>());
        var childDirs  = _roomDoors.GetValueOrDefault(childType,  new List<Direction>());
        return parentDirs.Contains(dir) && childDirs.Contains(DirectionUtil.Opposite(dir));
    }
}