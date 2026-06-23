using UnityEngine;
using UnityEngine.InputSystem;
 
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerStats baseStats;
    [SerializeField] private RunState    runState;
    [SerializeField] private GameObject  projectilePrefab;
    [SerializeField] private Transform   firePoint;

    // inside PlayerController class, add this field:
    [SerializeField] private bool canAttack = true;
 
    private CharacterController _cc;
    private Vector3             _moveDir;
    private float               _attackTimer;
    private bool                _canRoll = true;
    private bool                _isRolling;
    private float               _moveSpeed;
    private float               _attackDamage;
 
    private void Awake()
    {
        _cc           = GetComponent<CharacterController>();
        _moveSpeed    = baseStats.moveSpeed    * runState.speedMultiplier;
        _attackDamage = baseStats.attackDamage * runState.damageMultiplier;
    }
 
    private void Update()
    {
        ReadInput();
        Move();
        HandleAttack();
    }
 
    private void ReadInput()
    {
        var kb = Keyboard.current;
        if (kb == null) return;
        float x = (kb.dKey.isPressed ? 1 : 0) - (kb.aKey.isPressed ? 1 : 0);
        float z = (kb.wKey.isPressed ? 1 : 0) - (kb.sKey.isPressed ? 1 : 0);
        _moveDir = new Vector3(x, 0, z).normalized;
 
        if (kb.spaceKey.wasPressedThisFrame && _canRoll && !_isRolling && _moveDir != Vector3.zero)
            StartCoroutine(Roll());
    }
 
    private void Move()
    {
        if (_isRolling) return;
        _cc.Move(_moveDir * _moveSpeed * Time.deltaTime);
        if (_moveDir != Vector3.zero)
            transform.forward = _moveDir;
    }
 
    private void HandleAttack()
    {
        if (!canAttack) return;   // <-- add this
        _attackTimer -= Time.deltaTime;
        if (Mouse.current.leftButton.isPressed && _attackTimer <= 0f)
        {
            _attackTimer = baseStats.attackRate;
            FireProjectile();
        }
    }
 
    private void FireProjectile()
    {
        // Aim toward mouse world position
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        var dir = Vector3.forward;
        if (Physics.Raycast(ray, out var hit))
            dir = (hit.point - firePoint.position).normalized;
        dir.y = 0;
 
        var proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(dir));
        proj.GetComponent<Projectile>().Initialise(_attackDamage, baseStats.projectileSpeed, false);
    }
 
    private System.Collections.IEnumerator Roll()
    {
        _isRolling = true;
        _canRoll   = false;
 
        float elapsed  = 0f;
        var   rollDir  = _moveDir;
 
        while (elapsed < baseStats.rollDuration)
        {
            _cc.Move(rollDir * baseStats.rollSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
 
        _isRolling = false;
        yield return new WaitForSeconds(baseStats.rollCooldown);
        _canRoll = true;
    }
}