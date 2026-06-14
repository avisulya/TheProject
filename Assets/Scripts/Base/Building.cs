using UnityEngine;
 
public class Building : MonoBehaviour
{
    [SerializeField] private BuildingData data;
    [SerializeField] private GameState    gameState;
 
    public BuildingData Data => data;
 
    // Called by BaseTick every in-game day
    public void OnDayTick()
    {
        switch (data.buildingType)
        {
            case BuildingType.Farm:
                gameState.AddResource(ResourceType.Food, 5);
                GameEvents.ResourceChanged(ResourceType.Food, gameState.food);
                break;
 
            case BuildingType.Shrine:
                gameState.AddResource(ResourceType.Devotion, 3);
                GameEvents.ResourceChanged(ResourceType.Devotion, gameState.devotion);
                break;
 
            case BuildingType.Lumberyard:
                gameState.AddResource(ResourceType.Wood, 4);
                GameEvents.ResourceChanged(ResourceType.Wood, gameState.wood);
                break;
        }
    }
}