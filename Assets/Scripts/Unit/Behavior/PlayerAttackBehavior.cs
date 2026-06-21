// EnemyAttackBehavior.cs
using UnityEngine;

public class PlayerAttackBehavior : MonoBehaviour, IBehavior, IAttack
{
    [Header("Настройки атаки")]
    private float _attackCooldown;
    private float _damage;
    private float _attackDistance;
    [SerializeField] private float _searchRadius = 5f;
    
    private float _lastAttackTime;
    
    public float Behavior()
    {
        GameObject targetUnit = EnemyPool.instance.FindNearestTarget(transform.position, _searchRadius);
        
        if (targetUnit != null)
        {
            float distance = Vector2.Distance(transform.position, targetUnit.transform.position);
            if (distance <= _attackDistance)
            {
                return 0.9f;
            }
        }
        
        return 0f;
    }
    
    public void Realize()
    {
        if (Time.time - _lastAttackTime < _attackCooldown) return;
        
        GameObject targetUnit = EnemyPool.instance.FindNearestTarget(transform.position, _searchRadius);
        
        if (targetUnit == null) return;
        
        float distance = Vector2.Distance(transform.position, targetUnit.transform.position);
        
        if (distance <= _attackDistance)
        {
            Lives unitLives = targetUnit.GetComponent<Lives>();
            
            if (unitLives != null)
            {
                unitLives.GetDamage(_damage);
            }
            
            _lastAttackTime = Time.time;
        }
    }

    public void SetAttack(float newAttack)
    {
        _damage = newAttack;
    }

    public void SetAttackDistance(float newDistance)
    {
        _attackDistance = newDistance;
    }

    public void SetCoolDown(float newCoolDown)
    {
        _attackCooldown = newCoolDown;
    }
}
