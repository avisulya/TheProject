using System.Collections.Generic;
using UnityEngine;
 
[CreateAssetMenu(fileName = "MapGraph", menuName = "Map/Graph")]
public class MapGraph : ScriptableObject
{
    public MapNode       startNode;
    public List<MapNode> nodes = new();
    public List<MapEdge> edges = new();
 
    public List<MapNode> GetNeighbours(MapNode node)
    {
        var result = new List<MapNode>();
        foreach (var e in edges)
            if (e.from == node) result.Add(e.to);
        return result;
    }
}