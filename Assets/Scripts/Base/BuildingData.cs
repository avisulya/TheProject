using UnityEngine;
 
[CreateAssetMenu(fileName = "BuildingData", menuName = "Base/BuildingData")]
public class BuildingData : ScriptableObject
{
    public string       buildingName;
    public GameObject   prefab;
    public Sprite       icon;
    public int          costWood;
    public int          costStone;
    public BuildingType buildingType;
 
    [TextArea]
    public string description;
}