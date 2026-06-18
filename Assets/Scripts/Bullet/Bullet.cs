// Bullet.cs
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Настройки пули")]
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _lifeTime = 3f;
    
    private float _damage;
    private Vector2 _direction;
    private Rigidbody2D _rb;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            _rb = gameObject.AddComponent<Rigidbody2D>();
            _rb.gravityScale = 0;
        }
        Destroy(gameObject, _lifeTime);
    }
    
    private void FixedUpdate()
    {
        // Движение через Rigidbody для корректной работы коллизий
        _rb.velocity = _direction * _speed;
    }
    
    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction.normalized;
        
        // Поворот пули в направлении движения
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyLives unitLives = other.GetComponent<EnemyLives>();
        
        if (unitLives != null)
        {
            unitLives.GetDamage(_damage);
            Destroy(gameObject);
        }
    }
}
