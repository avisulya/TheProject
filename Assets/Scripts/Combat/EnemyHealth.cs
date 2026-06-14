using System;
using UnityEngine;
 
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private EnemyData data;
    [SerializeField] private GameState  gameState;
 
    public event Action OnDied;
 
    private int _currentHealth;
 
    private void Awake() => _currentHealth = data.maxHealth;
 
    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        if (_currentHealth <= 0)
            Die();
    }
 
    private void Die()
    {
        gameState.AddResource(ResourceType.Gold, data.goldReward);
        GameEvents.ResourceChanged(ResourceType.Gold, gameState.gold);
        OnDied?.Invoke();
        Destroy(gameObject);
    }
}