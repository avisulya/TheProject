// Assets/Scripts/Dungeon/DungeonManager.cs
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private RunState runState;

    [Header("Room Prefabs (one per RoomType)")]
    [SerializeField] private RoomData entryRoomData;
    [SerializeField] private RoomData combatRoomData;
    [SerializeField] private RoomData bossRoomData;
    [SerializeField] private RoomData treasureRoomData;
    [SerializeField] private RoomData shopRoomData;

    [Header("Enemies")]
    [SerializeField] private List<GameObject> commonEnemyPrefabs;
    [SerializeField] private List<GameObject> eliteEnemyPrefabs;

    [Header("Generation")]
    [SerializeField] private float roomSize          = 20f;   // matches your 20x20 floor
    [SerializeField] private int   criticalPathLength = 5;
    [SerializeField] private float branchChance       = 0.3f;

    [Header("Player")]
    [SerializeField] private GameObject playerPrefab;

    private Dictionary<RoomType, List<Direction>> _roomDoors;
    private DungeonNode _root;

    private void Start()
    {
        BuildDoorMap();
        GenerateDungeon();
    }

    // ── Door map: read each prefab's Room.doors to know what directions it supports ──

    private void BuildDoorMap()
    {
        _roomDoors = new Dictionary<RoomType, List<Direction>>();
        RegisterRoom(RoomType.Entry,    entryRoomData);
        RegisterRoom(RoomType.Combat,   combatRoomData);
        RegisterRoom(RoomType.Boss,     bossRoomData);
        RegisterRoom(RoomType.Treasure, treasureRoomData);
        RegisterRoom(RoomType.Shop,     shopRoomData);
    }

    private void RegisterRoom(RoomType type, RoomData data)
    {
        if (data == null || data.roomPrefab == null)
        {
            Debug.LogError($"No RoomData/prefab assigned for {type}");
            return;
        }
        var temp = Instantiate(data.roomPrefab);
        var room = temp.GetComponent<Room>();
        var dirs = new List<Direction>();
        foreach (var d in room.doors) dirs.Add(d.direction);
        _roomDoors[type] = dirs;
        Destroy(temp);
    }

    // ── Generation with retry, same pattern as your Godot version ──

    private void GenerateDungeon()
    {
        const int maxAttempts = 10;
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            _root = DungeonGenerator.Generate(criticalPathLength, branchChance, _roomDoors);
            if (_root != null)
            {
                BuildDungeon(_root, null, Vector3.zero);
                LinkAllTeleports(_root);
                SpawnPlayerAtStart();
                return;
            }
        }
        Debug.LogError($"Failed to generate dungeon after {maxAttempts} attempts.");
    }

    // ── Pass 1: instantiate + position every room ──

    private void BuildDungeon(DungeonNode node, DungeonNode parent, Vector3 parentWorldPos)
    {
        Vector3 worldPos;
        if (parent == null)
        {
            worldPos = Vector3.zero;
        }
        else
        {
            var exitWorld     = parentWorldPos + DoorOffset(node.parentExitDirection);
            var entranceLocal = DoorOffset(DirectionUtil.Opposite(node.parentExitDirection));
            worldPos = exitWorld - entranceLocal;
        }

        var data     = GetRoomData(node.roomType);
        var instance = Instantiate(data.roomPrefab, worldPos, Quaternion.identity);
        node.roomInstance = instance;

        var activeDirs = GetActiveDirections(node);
        var room       = instance.GetComponent<Room>();
        var enemies    = BuildEnemyList(node.roomType);
        room.Initialise(enemies, activeDirs);

        if (node.mainChild   != null) BuildDungeon(node.mainChild,   node, worldPos);
        if (node.branchChild != null) BuildDungeon(node.branchChild, node, worldPos);
    }

    private List<Direction> GetActiveDirections(DungeonNode node)
    {
        var dirs = new List<Direction>();
        if (node.parent != null)
            dirs.Add(DirectionUtil.Opposite(node.parentExitDirection));
        if (node.mainChild != null)
            dirs.Add(node.mainChild.parentExitDirection);
        if (node.branchChild != null)
            dirs.Add(node.branchChild.parentExitDirection);
        return dirs;
    }

    private List<GameObject> BuildEnemyList(RoomType type)
    {
        var list = new List<GameObject>();
        if (type != RoomType.Combat && type != RoomType.Elite) return list;

        var pool  = type == RoomType.Elite ? eliteEnemyPrefabs : commonEnemyPrefabs;
        int count = Random.Range(2, 5);
        for (int i = 0; i < count; i++)
            list.Add(pool[Random.Range(0, pool.Count)]);
        return list;
    }

    // ── Pass 2: open doors + wire DoorTP connections once everything exists ──

    private void LinkAllTeleports(DungeonNode node)
    {
        if (node == null) return;

        var room = node.roomInstance.GetComponent<Room>();
        var activeDirs = GetActiveDirections(node);
        room.OpenActiveDoors(activeDirs);

        foreach (var dir in activeDirs)
        {
            var myDoor = room.GetDoor(dir);
            if (myDoor == null || myDoor.Value.doorTP == null) continue;
            var myTP = myDoor.Value.doorTP.GetComponent<DoorTP>();

            bool isEntrance = node.parent != null
                            && dir == DirectionUtil.Opposite(node.parentExitDirection);

            DungeonNode neighbour = isEntrance ? node.parent
                                  : (node.mainChild   != null && node.mainChild.parentExitDirection == dir   ? node.mainChild
                                  : (node.branchChild != null && node.branchChild.parentExitDirection == dir ? node.branchChild
                                  : null));

            if (neighbour == null || neighbour.roomInstance == null) continue;

            var neighbourRoom = neighbour.roomInstance.GetComponent<Room>();
            var neighbourDir  = DirectionUtil.Opposite(dir);
            var neighbourDoor = neighbourRoom.GetDoor(neighbourDir);
            if (neighbourDoor == null || neighbourDoor.Value.doorTP == null) continue;

            myTP.connectedDoorTP = neighbourDoor.Value.doorTP.GetComponent<DoorTP>();
        }

        LinkAllTeleports(node.mainChild);
        LinkAllTeleports(node.branchChild);
    }

    // ── Helpers ──

    private RoomData GetRoomData(RoomType type) => type switch
    {
        RoomType.Entry    => entryRoomData,
        RoomType.Combat   => combatRoomData,
        RoomType.Boss     => bossRoomData,
        RoomType.Treasure => treasureRoomData,
        RoomType.Shop     => shopRoomData,
        _ => combatRoomData
    };

    private Vector3 DoorOffset(Direction dir)
    {
        float half = roomSize / 2f;
        return dir switch
        {
            Direction.North => new Vector3(0, 0, half),
            Direction.South => new Vector3(0, 0, -half),
            Direction.East  => new Vector3(half, 0, 0),
            Direction.West  => new Vector3(-half, 0, 0),
            _ => Vector3.zero
        };
    }

    private void SpawnPlayerAtStart()
    {
        var startRoom = _root.roomInstance.GetComponent<Room>();
        Instantiate(playerPrefab, startRoom.playerSpawnPoint.position, Quaternion.identity);
    }

    // Call this from the BOSS room's exit logic (e.g. after boss clear + player
    // walks to a designated dungeon-exit point, or immediately on boss clear)
    public void CompleteDungeon()
    {
        runState.MarkCleared(runState.currentNode);
        SceneTransition.LoadWorldMap();
    }

    private void OnEnable()  => GameEvents.OnDungeonCompleted += OnBossCleared;
    private void OnDisable() => GameEvents.OnDungeonCompleted -= OnBossCleared;

    private void OnBossCleared() => CompleteDungeon();
}