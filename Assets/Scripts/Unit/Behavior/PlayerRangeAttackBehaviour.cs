using UnityEngine;

public class PlayerRangeAttackBehaviour : MonoBehaviour, IBehavior, IAttack
{
    [Header("Настройки атаки")]
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _shootPoint;
    private float _attackCooldown;
    private float _damage;
    private float _searchRadius;
    
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

    public void SetAttack(float newAttack)
    {
        _damage = newAttack;
    }

    public void SetAttackDistance(float newDistance)
    {
        _searchRadius = newDistance;
    }

    public void SetCoolDown(float newCoolDown)
    {
        _attackCooldown = newCoolDown;
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
