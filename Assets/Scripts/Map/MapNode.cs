using UnityEngine;
 
[CreateAssetMenu(fileName = "MapNode", menuName = "Map/Node")]
public class MapNode : ScriptableObject
{
    public string   id;
    public NodeType nodeType;
    public string   sceneName;
    public Vector2  mapPosition;   // normalised 0-1 on the canvas
}