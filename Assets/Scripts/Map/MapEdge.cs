using UnityEngine;
 
[CreateAssetMenu(fileName = "MapEdge", menuName = "Map/Edge")]
public class MapEdge : ScriptableObject
{
    public MapNode from;
    public MapNode to;
}