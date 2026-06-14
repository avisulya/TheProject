using System.Collections.Generic;
using UnityEngine;
 
[CreateAssetMenu(fileName = "GameState", menuName = "Core/GameState")]
public class GameState : ScriptableObject
{
    [Header("Resources")]
    public int gold;
    public int devotion;       // currency earned from rituals/followers
    public int wood;
    public int stone;
    public int food;
 
    [Header("Progression")]
    public int playerLevel;
    public int currentXP;
    public int xpToNextLevel;
 
    [Header("Flags")]
    public bool isNewGame;
    public string lastScene;
 
    public void Reset()
    {
        gold        = 0;
        devotion    = 0;
        wood        = 50;
        stone       = 0;
        food        = 20;
        playerLevel = 1;
        currentXP   = 0;
        xpToNextLevel = 100;
        isNewGame   = true;
        lastScene   = "WorldMap";
    }
 
    public void AddResource(ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.Gold:     gold     += amount; break;
            case ResourceType.Devotion: devotion += amount; break;
            case ResourceType.Wood:     wood     += amount; break;
            case ResourceType.Stone:    stone    += amount; break;
            case ResourceType.Food:     food     += amount; break;
        }
    }
 
    public bool SpendResource(ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.Gold:
                if (gold < amount) return false;
                gold -= amount; return true;
            case ResourceType.Wood:
                if (wood < amount) return false;
                wood -= amount; return true;
            case ResourceType.Stone:
                if (stone < amount) return false;
                stone -= amount; return true;
            case ResourceType.Food:
                if (food < amount) return false;
                food -= amount; return true;
            case ResourceType.Devotion:
                if (devotion < amount) return false;
                devotion -= amount; return true;
        }
        return false;
    }
}