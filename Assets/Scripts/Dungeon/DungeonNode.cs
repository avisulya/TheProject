// Assets/Scripts/Dungeon/DungeonNode.cs
using UnityEngine;

public class DungeonNode
{
    public RoomType   roomType;
    public Vector2Int gridPosition;
    public DungeonNode parent;
    public DungeonNode mainChild;
    public DungeonNode branchChild;
    public Direction   parentExitDirection;   // direction from parent INTO this node

    // Runtime, set by DungeonManager after instancing
    public GameObject roomInstance;
}