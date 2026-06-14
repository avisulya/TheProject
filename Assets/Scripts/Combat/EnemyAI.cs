using UnityEngine;
 
[RequireComponent(typeof(CharacterController))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private EnemyData      data;
    [SerializeField] private GameObject     projectilePrefab;
    [SerializeField] private Transform      firePoint;
 
    private CharacterController _cc;
    private Transform           _player;
    private float               _attackTimer;
 
    private void Awake() => _cc = GetComponent<CharacterController>();
 
    private void Start()
    {
        var p = FindObjectOfType<PlayerController>();
        if (p != null) _player = p.transform;
    }
 
    private void Update()
    {
        if (_player == null) return;
 
        float dist = Vector3.Distance(transform.position, _player.position);
 
        if (dist > data.detectionRange) return;
 
        // Face player
        var dir = (_player.position - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero) transform.forward = dir;
 
        // Move toward player if outside attack range
        if (dist > data.attackRange)
            _cc.Move(dir * data.moveSpeed * Time.deltaTime);
 
        // Attack
        _attackTimer -= Time.deltaTime;
        if (dist <= data.attackRange && _attackTimer <= 0f)
        {
            _attackTimer = data.attackCooldown;
            Attack(dir);
        }
    }
 
    private void Attack(Vector3 dir)
    {
        if (projectilePrefab == null) return;
        var proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(dir));
        proj.GetComponent<Projectile>().Initialise(data.attackDamage, 8f, true);
    }
}