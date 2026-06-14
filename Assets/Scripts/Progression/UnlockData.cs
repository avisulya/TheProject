using UnityEngine;
 
[CreateAssetMenu(fileName = "UnlockData", menuName = "Progression/UnlockData")]
public class UnlockData : ScriptableObject
{
    public string        unlockId;
    public string        unlockName;
    public int           devotioncost;
    public BuildingData  buildingUnlock;   // null if not a building unlock
    public TarotData     tarotUnlock;      // null if not a tarot unlock
}