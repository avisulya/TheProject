// File: Room.cs
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Configuration")]
    public RoomType roomType;

    [Header("Scene references")]
    public Transform        enemySpawnParent;
    public Transform        playerSpawnPoint;
    public List<Transform>  enemySpawnPoints;

    [Header("Door Map — which directions this room has doors for")]
    public List<DoorEntry> doors;   // see struct below

    [HideInInspector] public bool isCleared;

    private int _enemiesAlive;

    [System.Serializable]
    public struct DoorEntry
    {
        public Direction  direction;
        public GameObject doorBarrier;   // the DoorBarrier object for this direction
        public Transform  doorTP;        // the TP_X transform for this direction
    }

    public bool HasDoor(Direction dir)
    {
        foreach (var d in doors) if (d.direction == dir) return true;
        return false;
    }

    public DoorEntry? GetDoor(Direction dir)
    {
        foreach (var d in doors) if (d.direction == dir) return d;
        return null;
    }

    public void Initialise(List<GameObject> enemyPrefabs, List<Direction> activeDoors)
    {
        _enemiesAlive = 0;
        isCleared = roomType != RoomType.Combat
                 && roomType != RoomType.Elite
                 && roomType != RoomType.Boss;

        // Lock only the doors that are part of this dungeon's layout.
        // Doors NOT in activeDoors stay permanently closed (never toggled, never opened).
        CloseActiveDoors(activeDoors);

        if (roomType == RoomType.Boss)
        {
            var boss = GetComponentInChildren<BossAI>();
            if (boss != null)
                GameEvents.OnDungeonCompleted += OnBossDefeated;
            return;
        }

        if (!isCleared)
        {
            foreach (var prefab in enemyPrefabs)
            {
                var pt    = enemySpawnPoints[_enemiesAlive % enemySpawnPoints.Count];
                var enemy = Instantiate(prefab, pt.position,
                                        Quaternion.identity, enemySpawnParent);
                enemy.GetComponent<EnemyHealth>().OnDied += OnEnemyDied;
                _enemiesAlive++;
            }
        }
        else
        {
            OpenActiveDoors(activeDoors);
        }
    }

    private void OnBossDefeated()
    {
        GameEvents.OnDungeonCompleted -= OnBossDefeated;
        ClearRoom();
    }

    private void OnEnemyDied()
    {
        _enemiesAlive--;
        if (_enemiesAlive <= 0) ClearRoom();
    }

    private void ClearRoom()
    {
        isCleared = true;
        GameEvents.RoomCleared();
    }

    private void CloseActiveDoors(List<Direction> activeDoors)
    {
        foreach (var dir in activeDoors)
        {
            var entry = GetDoor(dir);
            if (entry.HasValue) entry.Value.doorBarrier.SetActive(true);
        }
    }

    public void OpenActiveDoors(List<Direction> activeDoors)
    {
        foreach (var dir in activeDoors)
        {
            var entry = GetDoor(dir);
            if (entry.HasValue) entry.Value.doorBarrier.SetActive(false);
        }
    }
}