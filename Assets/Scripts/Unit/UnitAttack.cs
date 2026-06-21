// UnitAttack.cs
using UnityEngine;

public class UnitAttack : MonoBehaviour
{
    [Header("Атака")]
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private int _damage = 10;
    
    private GameObject _targetUnit;
    private float _lastAttackTime;
    private UnitMovement _unitMovement;

    private void Start()
    {
        _unitMovement = GetComponent<UnitMovement>();
    }

    private void Update()
    {
        if (_targetUnit == null)
            return;

        float distanceToTarget = Vector3.Distance(transform.position, _targetUnit.transform.position);

        // Проверяем, может ли цель быть атакована (она не была уничтожена)
        if (_targetUnit.activeInHierarchy == false)
        {
            _targetUnit = null;
            _unitMovement.Stop();
            return;
        }

        if (distanceToTarget <= _attackRange)
        {
            // Близко к цели - атакуем
            _unitMovement.Stop();
            Attack();
        }
        else
        {
            // Далеко от цели - приближаемся
            _unitMovement.MoveTo(_targetUnit.transform.position);
        }
    }

    /// <summary>
    /// Приказ атаковать юнита
    /// </summary>
    public void AttackUnit(GameObject target)
    {
        _targetUnit = target;
        Debug.Log($"Атакую {target.name}");
    }

    /// <summary>
    /// Выполнить атаку
    /// </summary>
    private void Attack()
    {
        if (Time.time - _lastAttackTime < _attackCooldown)
            return;

        if (_targetUnit == null)
            return;

        Lives targetLives = _targetUnit.GetComponent<Lives>();
        if (targetLives != null)
        {
            targetLives.GetDamage(_damage);
            Debug.Log($"Атаковал! Урон: {_damage}");
        }

        _lastAttackTime = Time.time;
    }

    /// <summary>
    /// Остановить атаку
    /// </summary>
    public void StopAttack()
    {
        _targetUnit = null;
    }

    /// <summary>
    /// Получить текущую цель
    /// </summary>
    public GameObject GetTarget()
    {
        return _targetUnit;
    }

    public void SetAttackRange(float range)
    {
        _attackRange = range;
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }
}
