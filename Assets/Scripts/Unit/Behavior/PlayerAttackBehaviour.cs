using UnityEngine;

public class PlayerAttackBehaviour : MonoBehaviour, IBehavior
{
    [Header("Настройки атаки")]
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private int _damage = 15;
    
    [Header("Поиск цели")]
    [SerializeField] private float _searchRadius = 5f;
    
    private Transform _currentTarget;
    private float _lastAttackTime;
    
    public float Behavior()
    {
        _currentTarget = EnemyPool.instance.FindNearestTarget(transform.position, _searchRadius)?.transform;
        if (_currentTarget == null) return 0f;
        else return 1f;
    }
    
    public void Realize()
    {
        if (_currentTarget != null && Time.time - _lastAttackTime >= _attackCooldown)
        {
            Shoot();
            _lastAttackTime = Time.time;
        }
    }
    
    private void Shoot()
    {
        if (_bullet == null) return;
        
        GameObject newBullet = Instantiate(_bullet, _shootPoint.position, Quaternion.identity);
        
        Bullet bulletScript = newBullet.GetComponent<Bullet>();
        bulletScript.SetDamage(_damage);
        
        // Вычисляем направление от пули к цели
        Vector2 directionToTarget = (_currentTarget.position - _shootPoint.position).normalized;
        bulletScript.SetDirection(directionToTarget);
    }
}
