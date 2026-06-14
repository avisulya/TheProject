using System;
using UnityEngine;
 
public static class GameEvents
{
    // Resources
    public static event Action<ResourceType, int> OnResourceChanged;
    public static void ResourceChanged(ResourceType type, int newValue)
        => OnResourceChanged?.Invoke(type, newValue);
 
    // Combat
    public static event Action<int, int> OnPlayerHealthChanged;   // current, max
    public static void PlayerHealthChanged(int current, int max)
        => OnPlayerHealthChanged?.Invoke(current, max);
 
    public static event Action OnPlayerDied;
    public static void PlayerDied() => OnPlayerDied?.Invoke();
 
    public static event Action OnRoomCleared;
    public static void RoomCleared() => OnRoomCleared?.Invoke();
 
    public static event Action OnDungeonCompleted;
    public static void DungeonCompleted() => OnDungeonCompleted?.Invoke();
 
    // Followers
    public static event Action<FollowerData> OnFollowerAdded;
    public static void FollowerAdded(FollowerData f) => OnFollowerAdded?.Invoke(f);
 
    public static event Action<FollowerData> OnFollowerDied;
    public static void FollowerDied(FollowerData f) => OnFollowerDied?.Invoke(f);
 
    // Base
    public static event Action OnBaseTick;
    public static void BaseTick() => OnBaseTick?.Invoke();
 
    // Progression
    public static event Action<int> OnLevelUp;
    public static void LevelUp(int newLevel) => OnLevelUp?.Invoke(newLevel);
}