using System.Collections.Generic;
using UnityEngine;
 
[CreateAssetMenu(fileName = "RunState", menuName = "Run/RunState")]
public class RunState : ScriptableObject
{
    public MapGraph      graph;
    public MapNode       currentNode;
    public List<string>  clearedNodeIds = new();
 
    // Tarot cards collected this run
    public List<string>  activeTarotIds = new();
 
    // Stat modifiers accumulated this run
    public float damageMultiplier  = 1f;
    public float speedMultiplier   = 1f;
    public int   extraHearts       = 0;
 
    public bool IsCleared(MapNode n) => clearedNodeIds.Contains(n.id);
 
    public void MarkCleared(MapNode n)
    {
        if (!IsCleared(n)) clearedNodeIds.Add(n.id);
    }
 
    public void StartNewRun(MapGraph g)
    {
        graph              = g;
        currentNode        = g.startNode;
        clearedNodeIds.Clear();
        activeTarotIds.Clear();
        damageMultiplier   = 1f;
        speedMultiplier    = 1f;
        extraHearts        = 0;
    }
}