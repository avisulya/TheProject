using UnityEngine;
 
public class Projectile : MonoBehaviour
{
    private float  _damage;
    private float  _speed;
    private bool   _fromEnemy;
    private float  _lifetime = 4f;
 
    public void Initialise(float damage, float speed, bool fromEnemy)
    {
        _damage    = damage;
        _speed     = speed;
        _fromEnemy = fromEnemy;
        Destroy(gameObject, _lifetime);
    }
 
    private void Update()
        => transform.position += transform.forward * _speed * Time.deltaTime;
 
    private void OnTriggerEnter(Collider other)
    {
        if (_fromEnemy)
        {
            var playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(Mathf.RoundToInt(_damage));
                Destroy(gameObject);
            }
        }
        else
        {
            var enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(Mathf.RoundToInt(_damage));
                Destroy(gameObject);
            }
        }
 
        // Destroy on walls
        if (other.CompareTag("Wall"))
            Destroy(gameObject);
    }
}