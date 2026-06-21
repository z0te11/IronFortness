// UnitMovement.cs
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [Header("Движение")]
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _stoppingDistance = 0.2f;
    
    [Header("Поворот")]
    [SerializeField] private bool _flipSpriteOnly = true; // true = только отражение спрайта, false = поворот тела
    
    private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Vector3 _targetPosition;
    [HideInInspector] public bool isMoving = false;
    private float _lastDirectionX = 1f; // 1 = вправо, -1 = влево

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            _rb = gameObject.AddComponent<Rigidbody2D>();
            _rb.gravityScale = 0;
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            MoveTowardsTarget();
        }
    }

    /// <summary>
    /// Приказ идти на позицию
    /// </summary>
    public void MoveTo(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
        isMoving = true;
    }

    /// <summary>
    /// Движение к цели
    /// </summary>
    private void MoveTowardsTarget()
    {
        float distance = Vector3.Distance(transform.position, _targetPosition);

        if (distance <= _stoppingDistance)
        {
            // Достигли цели
            _rb.velocity = Vector2.zero;
            isMoving = false;
            return;
        }

        // Направление к цели
        Vector3 direction = (_targetPosition - transform.position).normalized;
        
        // Двигаемся
        _rb.velocity = new Vector2(direction.x, direction.y) * _moveSpeed;

        // Поворачиваемся влево или вправо
        FaceDirection(direction.x);
    }

    /// <summary>
    /// Поворот персонажа влево или вправо
    /// </summary>
    private void FaceDirection(float directionX)
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

    /// <summary>
    /// Повернуть в определённую сторону (1 = вправо, -1 = влево)
    /// </summary>
    public void SetFacingDirection(float direction)
    {
        FaceDirection(direction);
    }

    /// <summary>
    /// Получить текущее направление (1 = вправо, -1 = влево)
    /// </summary>
    public float GetFacingDirection()
    {
        return _lastDirectionX;
    }

    /// <summary>
    /// Остановить движение
    /// </summary>
    public void Stop()
    {
        isMoving = false;
        _rb.velocity = Vector2.zero;
    }

    /// <summary>
    /// Проверить, движется ли юнит
    /// </summary>
    public bool IsMoving()
    {
        return isMoving;
    }

    public void SetMoveSpeed(float speed)
    {
        _moveSpeed = speed;
    }
}
