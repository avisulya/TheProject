using System.Collections.Generic;
using UnityEngine;
 
public class FollowerManager : MonoBehaviour
{
    [SerializeField] private GameObject     followerPrefab;
    [SerializeField] private List<Transform> spawnPoints;
 
    private readonly List<Follower> _followers = new();
 
    private void OnEnable()  => GameEvents.OnFollowerAdded += SpawnFollower;
    private void OnDisable() => GameEvents.OnFollowerAdded -= SpawnFollower;
 
    private void SpawnFollower(FollowerData data)
    {
        var pt      = spawnPoints[_followers.Count % spawnPoints.Count];
        var obj     = Instantiate(followerPrefab, pt.position, Quaternion.identity);
        var follower = obj.GetComponent<Follower>();
        _followers.Add(follower);
    }
 
    public void RemoveFollower(Follower f) => _followers.Remove(f);
 
    public List<Follower> GetAll() => _followers;
}