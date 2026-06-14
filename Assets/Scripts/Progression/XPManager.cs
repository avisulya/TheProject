using UnityEngine;
 
public class XPManager : MonoBehaviour
{
    [SerializeField] private GameState gameState;
 
    private void OnEnable()  => GameEvents.OnRoomCleared += OnRoomCleared;
    private void OnDisable() => GameEvents.OnRoomCleared -= OnRoomCleared;
 
    private void OnRoomCleared()
    {
        AddXP(25);
    }
 
    public void AddXP(int amount)
    {
        gameState.currentXP += amount;
        while (gameState.currentXP >= gameState.xpToNextLevel)
        {
            gameState.currentXP    -= gameState.xpToNextLevel;
            gameState.playerLevel++;
            gameState.xpToNextLevel = Mathf.RoundToInt(gameState.xpToNextLevel * 1.4f);
            GameEvents.LevelUp(gameState.playerLevel);
        }
    }
}