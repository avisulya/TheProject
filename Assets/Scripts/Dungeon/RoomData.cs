using UnityEngine;
 
[CreateAssetMenu(fileName = "RoomData", menuName = "Dungeon/RoomData")]
public class RoomData : ScriptableObject
{
    public RoomType     roomType;
    public GameObject   roomPrefab;
    public int          minEnemies;
    public int          maxEnemies;
}