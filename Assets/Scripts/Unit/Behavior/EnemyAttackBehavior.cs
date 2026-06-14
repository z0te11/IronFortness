// EnemyAttackBehavior.cs
using UnityEngine;

public class EnemyAttackBehavior : MonoBehaviour, IBehavior
{
    [Header("Настройки атаки")]
    [SerializeField] private float _attackCooldown = 1.5f;
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _attackDistance = 0.5f;
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
            UnitPlayerLives unitLives = playerUnit.GetComponent<UnitPlayerLives>();
            
            if (unitLives != null)
            {
                unitLives.GetDamage(_damage);
            }
            
            _lastAttackTime = Time.time;
        }
    }
}
