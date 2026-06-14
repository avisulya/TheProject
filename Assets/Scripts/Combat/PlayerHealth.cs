using UnityEngine;
 
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerStats baseStats;
    [SerializeField] private RunState    runState;
 
    private int _currentHealth;
    private int _maxHealth;
    private bool _invincible;
 
    private void Awake()
    {
        _maxHealth     = baseStats.maxHealth + runState.extraHearts;
        _currentHealth = _maxHealth;
        GameEvents.PlayerHealthChanged(_currentHealth, _maxHealth);
    }
 
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
 
        // Brief invincibility window
        StartCoroutine(InvincibilityFrames(0.5f));
    }
 
    public void Heal(int amount)
    {
        _currentHealth = Mathf.Min(_maxHealth, _currentHealth + amount);
        GameEvents.PlayerHealthChanged(_currentHealth, _maxHealth);
    }
 
    private System.Collections.IEnumerator InvincibilityFrames(float duration)
    {
        _invincible = true;
        yield return new WaitForSeconds(duration);
        _invincible = false;
    }
}