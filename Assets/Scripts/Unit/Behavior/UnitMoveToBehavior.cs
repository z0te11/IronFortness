// EnemyMoveToPlayerBehavior.cs
using System.Collections;
using UnityEngine;

public class UnitMoveToBehavior : MonoBehaviour, IBehavior
{
    [Header("Настройки движения")]
    [SerializeField] protected float _speed = 2f;
    [SerializeField] protected float _searchRadius = 5f;
    [SerializeField] protected TypeUnit _findTypeUnit;
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [Header("Поворот")]
    [SerializeField] protected bool _flipSpriteOnly = true;
    protected float _lastDirectionX = 1f; // 1 = вправо, -1 = влево
    protected GameObject _targetUnit;
    protected Rigidbody2D _rb;
    
    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            _rb = gameObject.AddComponent<Rigidbody2D>();
            _rb.gravityScale = 0;
        }
    }
    
    public virtual float Behavior()
    {
        switch (_findTypeUnit)
        {
            case TypeUnit.Player:
                {
                    _targetUnit = PlayerPool.instance.FindNearestPlayerUnit(transform.position, _searchRadius);
                    break;
                }
            case TypeUnit.Enemy:
                {
                    _targetUnit = EnemyPool.instance.FindNearestTarget(transform.position, _searchRadius);
                    break;
                }
            
        }
        
        if (_targetUnit != null)
        {
            return 0.8f;
        }
        
        return 0f;
    }
    
    public virtual void Realize()
    {   
        if (_targetUnit == null) return;
        
        Vector2 direction = (_targetUnit.transform.position - transform.position).normalized;
        _rb.velocity = direction * _speed;
        FaceDirection(direction.x);
    }

    protected void FaceDirection(float directionX)
    {
        if (Mathf.Abs(directionX) < 0.01f)
            return; // Движение почти вверх/вниз - не меняем поворот

        float targetDirectionX = directionX > 0 ? 1f : -1f;

        if (_flipSpriteOnly)
        {
            // Отражаем спрайт (не поворачиваем тело)
            if (_spriteRenderer != null)
            {
                _spriteRenderer.flipX = targetDirectionX < 0;
            }
        }
        else
        {
            // Поворачиваем весь объект
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * targetDirectionX;
            transform.localScale = scale;
        }

        _lastDirectionX = targetDirectionX;
    }

    // В EnemyMovement.cs нужно добавить этот метод:
    public void ApplySlow(float slowAmount, float duration)
    {
        StartCoroutine(SlowCoroutine(slowAmount, duration));
    }

    private IEnumerator SlowCoroutine(float slowAmount, float duration)
    {
        float originalSpeed = _speed;
        _speed *= slowAmount;
        
        yield return new WaitForSeconds(duration);
        
        _speed = originalSpeed;
    }
}
