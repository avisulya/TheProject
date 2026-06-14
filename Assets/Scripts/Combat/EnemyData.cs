using UnityEngine;
 
[CreateAssetMenu(fileName = "EnemyData", menuName = "Combat/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string  enemyName;
    public int     maxHealth      = 30;
    public float   moveSpeed      = 3f;
    public float   attackDamage   = 5f;
    public float   attackRange    = 1.5f;
    public float   attackCooldown = 1.5f;
    public float   detectionRange = 8f;
    public int     xpReward       = 10;
    public int     goldReward     = 5;
}