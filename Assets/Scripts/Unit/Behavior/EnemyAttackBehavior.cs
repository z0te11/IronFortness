// EnemyAttackBehavior.cs
using UnityEngine;

public class EnemyAttackBehavior : MonoBehaviour, IBehavior, IAttack
{
    [Header("Настройки атаки")]
    private float _attackCooldown = 0f;
    private float _damage = 0f;
    private float _attackDistance = 0f;
    [SerializeField] private float _searchRadius = 5f;
    
    private float _lastAttackTime;
    
    public float Behavior()
    {
        GameObject playerUnit = PlayerPool.instance.FindNearestPlayerUnit(transform.position, _searchRadius);
        
        if (playerUnit != null)
        {
            float distance = Vector2.Distance(transform.position, playerUnit.transform.position);
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
        
        GameObject playerUnit = PlayerPool.instance.FindNearestPlayerUnit(transform.position, _searchRadius);
        
        if (playerUnit == null) return;
        
        float distance = Vector2.Distance(transform.position, playerUnit.transform.position);
        
        if (distance <= _attackDistance)
        {
            PlayerLives unitLives = playerUnit.GetComponent<PlayerLives>();
            
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
