using UnityEngine;
 
[CreateAssetMenu(fileName = "PlayerStats", menuName = "Combat/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public int   maxHealth      = 6;
    public float moveSpeed      = 6f;
    public float rollSpeed      = 14f;
    public float rollDuration   = 0.18f;
    public float rollCooldown   = 0.8f;
    public float attackDamage   = 10f;
    public float attackRate     = 0.4f;    // seconds between attacks
    public float projectileSpeed = 12f;
}