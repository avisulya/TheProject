using System.Collections.Generic;
using UnityEngine;
 
public class RoomManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private RunState       runState;
    [SerializeField] private List<RoomData> combatRooms;
    [SerializeField] private List<RoomData> eliteRooms;
    [SerializeField] private List<RoomData> treasureRooms;
    [SerializeField] private RoomData       exitRoomData;
    [SerializeField] private List<GameObject> commonEnemyPrefabs;
    [SerializeField] private List<GameObject> eliteEnemyPrefabs;
 
    [Header("Config")]
    [SerializeField] private int roomsPerRun = 4;
 
    [Header("Player")]
    [SerializeField] private GameObject playerPrefab;
 
    private Room          _currentRoom;
    private GameObject    _currentRoomInstance;
    private int           _roomIndex;
    private List<RoomData> _sequence;
 
    private void Start()
    {
        BuildSequence();
        LoadRoom(0);
    }
 
    private void BuildSequence()
    {
        _sequence = new List<RoomData>();
        for (int i = 0; i < roomsPerRun - 1; i++)
        {
            bool elite = Random.value < 0.25f;
            var pool   = elite ? eliteRooms : combatRooms;
            _sequence.Add(pool[Random.Range(0, pool.Count)]);
        }
        _sequence.Add(exitRoomData);
    }
 
    private void LoadRoom(int index)
    {
        _roomIndex = index;
 
        if (_currentRoomInstance != null)
            Destroy(_currentRoomInstance);
 
        var data    = _sequence[index];
        _currentRoomInstance = Instantiate(data.roomPrefab, Vector3.zero, Quaternion.identity);
        _currentRoom         = _currentRoomInstance.GetComponent<Room>();
 
        var enemies = BuildEnemyList(data);
        _currentRoom.Initialise(enemies);
 
        SpawnPlayer(_currentRoom.playerSpawnPoint.position);
 
        GameEvents.OnRoomCleared += OnRoomCleared;
    }
 
    private List<GameObject> BuildEnemyList(RoomData data)
    {
        var list  = new List<GameObject>();
        var pool  = data.roomType == RoomType.Elite ? eliteEnemyPrefabs : commonEnemyPrefabs;
        int count = Random.Range(data.minEnemies, data.maxEnemies + 1);
        for (int i = 0; i < count; i++)
            list.Add(pool[Random.Range(0, pool.Count)]);
        return list;
    }
 
    private void SpawnPlayer(Vector3 pos)
    {
        var existing = FindObjectOfType<PlayerController>();
        if (existing != null)
        {
            existing.transform.position = pos;
            return;
        }
        Instantiate(playerPrefab, pos, Quaternion.identity);
    }
 
    private void OnRoomCleared()
    {
        GameEvents.OnRoomCleared -= OnRoomCleared;
 
        int next = _roomIndex + 1;
        if (next < _sequence.Count)
            LoadRoom(next);
        else
            DungeonCompleted();
    }
 
    private void DungeonCompleted()
    {
        runState.MarkCleared(runState.currentNode);
        GameEvents.DungeonCompleted();
        // Small delay then return to map
        Invoke(nameof(ReturnToMap), 2f);
    }
 
    private void ReturnToMap() => SceneTransition.LoadWorldMap();
}