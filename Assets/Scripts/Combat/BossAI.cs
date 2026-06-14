using System.Collections;
using UnityEngine;
 
public class BossAI : MonoBehaviour
{
    [SerializeField] private EnemyData  data;
    [SerializeField] private int        maxHealth = 300;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform  firePoint;
    [SerializeField] private int        phase2Threshold = 200;
    [SerializeField] private int        phase3Threshold = 100;
 
    private int       _currentHealth;
    private int       _phase = 1;
    private Transform _player;
    private float     _attackTimer;
 
    private void Start()
    {
        _currentHealth = maxHealth;
        var p = FindObjectOfType<PlayerController>();
        if (p != null) _player = p.transform;
        StartCoroutine(AttackLoop());
    }
 
    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        CheckPhaseTransition();
        if (_currentHealth <= 0) Die();
    }
 
    private void CheckPhaseTransition()
    {
        if (_phase == 1 && _currentHealth <= phase2Threshold) EnterPhase(2);
        else if (_phase == 2 && _currentHealth <= phase3Threshold) EnterPhase(3);
    }
 
    private void EnterPhase(int phase)
    {
        _phase = phase;
        Debug.Log($"Boss entering phase {phase}");
        // Add phase-specific visual/audio cues here
    }
 
    private IEnumerator AttackLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(data.attackCooldown / _phase);
            if (_player == null) yield break;
 
            switch (_phase)
            {
                case 1: FireSingle(); break;
                case 2: FireSpread(3); break;
                case 3: FireSpread(5); break;
            }
        }
    }
 
    private void FireSingle()
    {
        var dir = (_player.position - firePoint.position).normalized;
        dir.y = 0;
        Spawn(dir);
    }
 
    private void FireSpread(int count)
    {
        var baseDir = (_player.position - firePoint.position).normalized;
        baseDir.y   = 0;
        float step  = 15f;
        float start = -(count / 2) * step;
        for (int i = 0; i < count; i++)
        {
            var rot = Quaternion.Euler(0, start + i * step, 0);
            Spawn(rot * baseDir);
        }
    }
 
    private void Spawn(Vector3 dir)
    {
        var proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(dir));
        proj.GetComponent<Projectile>().Initialise(data.attackDamage, 10f, true);
    }
 
    private void Die()
    {
        GameEvents.DungeonCompleted();
        Destroy(gameObject, 0.5f);
    }
}