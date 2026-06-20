// Assets/Scripts/Combat/PlayerHealth.cs
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerStats baseStats;
    [SerializeField] private RunState    runState;

    private int  _currentHealth;   // in HALF-HEART units
    private int  _maxHealth;       // in HALF-HEART units
    private bool _invincible;

    private void Awake()
    {
        _maxHealth     = (baseStats.maxHealth + runState.extraHearts) * 2;
        _currentHealth = _maxHealth;
        GameEvents.PlayerHealthChanged(_currentHealth, _maxHealth);
    }

    // amount is in HALF-HEART units (1 = half heart, 2 = full heart)
    public void TakeDamage(int amount)
    {
        if (_invincible) return;
        _currentHealth = Mathf.Max(0, _currentHealth - amount);
        GameEvents.PlayerHealthChanged(_currentHealth, _maxHealth);
        if (_currentHealth <= 0)
        {
            GameEvents.PlayerDied();
            Destroy(gameObject);
            return;
        }
        StartCoroutine(InvincibilityFrames(0.5f));
    }

    public void Heal(int amount)
    {
        _currentHealth = Mathf.Min(_maxHealth, _currentHealth + amount);
        GameEvents.PlayerHealthChanged(_currentHealth, _maxHealth);
    }

    private IEnumerator InvincibilityFrames(float duration)
    {
        _invincible = true;
        yield return new WaitForSeconds(duration);
        _invincible = false;
    }
}