using System.Collections.Generic;
using UnityEngine;
 
public class Room : MonoBehaviour
{
    [Header("Configuration")]
    public RoomType roomType;
 
    [Header("Scene references")]
    public Transform        enemySpawnParent;    // parent for spawned enemies
    public Transform        playerSpawnPoint;
    public List<GameObject> doorObjects;         // walls that open on clear
    public List<Transform>  enemySpawnPoints;
 
    [HideInInspector] public bool isCleared;
 
    private int _enemiesAlive;
 
    public void Initialise(List<GameObject> enemyPrefabs)
    {
        _enemiesAlive = 0;
        isCleared     = roomType != RoomType.Combat && roomType != RoomType.Elite;
 
        if (!isCleared)
        {
            CloseDoors();
            foreach (var prefab in enemyPrefabs)
            {
                var spawnPt = enemySpawnPoints[_enemiesAlive % enemySpawnPoints.Count];
                var enemy   = Instantiate(prefab, spawnPt.position, Quaternion.identity, enemySpawnParent);
                enemy.GetComponent<EnemyHealth>().OnDied += OnEnemyDied;
                _enemiesAlive++;
            }
        }
    }
 
    private void OnEnemyDied()
    {
        _enemiesAlive--;
        if (_enemiesAlive <= 0)
            ClearRoom();
    }
 
    private void ClearRoom()
    {
        isCleared = true;
        OpenDoors();
        GameEvents.RoomCleared();
    }
 
    private void CloseDoors()
    {
        foreach (var d in doorObjects) d.SetActive(true);
    }
 
    private void OpenDoors()
    {
        foreach (var d in doorObjects) d.SetActive(false);
    }
}