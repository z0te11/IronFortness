using UnityEngine;

/// <summary>
/// Компонент движения игрока. Получает направление из InputHandler и двигает объект через Transform.
/// Только для игрока. Без физики. Ничего больше не делает!
/// </summary>
public class PlayerMover : MonoBehaviour
{
    [Header("Настройки движения")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float minSpeed = 0.1f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private bool useSmoothing = true;
    [SerializeField] private float smoothingFactor = 0.1f;
    
    [Header("Анимация")]
    [SerializeField] private bool useAnimator = true;
    [SerializeField] private string moveXParameter = "moveX";
    [SerializeField] private string moveYParameter = "moveY";
    [SerializeField] private string isMovingParameter = "isMoving";
    
    [Header("Ссылки")]
    [SerializeField] private InputHandler inputHandler;
    
    private Animator animator;
    private Vector2 currentVelocity;
    private Vector2 lastDirection = Vector2.down;
    private bool isMoving = false;
    
    private void Awake()
    {
        // Если не назначен в инспекторе — ищем на этом же объекте
        if (inputHandler == null)
            inputHandler = GetComponent<InputHandler>();
        
        // Если всё равно не нашли — ищем в родителях
        if (inputHandler == null)
            inputHandler = GetComponentInParent<InputHandler>();
        
        // Ищем Animator если нужен
        if (useAnimator)
            animator = GetComponent<Animator>();
        
        // Валидация
        ValidateSettings();
    }
    
    private void OnEnable()
    {
        if (inputHandler != null)
        {
            inputHandler.OnMoveInput += HandleMove;
        }
        else
        {
            Debug.LogWarning($"PlayerMover на {gameObject.name}: нет InputHandler, движение не будет работать");
        }
    }
    
    private void OnDisable()
    {
        if (inputHandler != null)
        {
            inputHandler.OnMoveInput -= HandleMove;
        }
        
        Stop();
    }
    
    private void Update()
    {
        if (inputHandler == null || !inputHandler.InputEnabled)
            return;
        
        // Двигаем игрока каждый кадр
        MovePlayer();
    }
    
    /// <summary>
    /// Движение игрока по миру
    /// </summary>
    private void MovePlayer()
    {
        // Применяем сглаживание если включено
        if (useSmoothing)
        {
            currentVelocity = Vector2.Lerp(currentVelocity, inputHandler.GetRawMoveDirection() * speed, smoothingFactor);
        }
        
        // Двигаем объект в мире
        transform.Translate((Vector3)currentVelocity * Time.deltaTime, Space.World);
    }
    
    /// <summary>
    /// Обработчик события движения от InputHandler
    /// </summary>
    private void HandleMove(Vector2 direction)
    {
        // Обновляем скорость
        if (!useSmoothing)
        {
            currentVelocity = direction * speed;
        }
        
        // Обновляем последнее направление для анимации
        if (direction.sqrMagnitude > 0.01f)
        {
            lastDirection = direction;
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        
        // Передаём в Animator
        UpdateAnimator(direction);
    }
    
    /// <summary>
    /// Обновление параметров Animator
    /// </summary>
    private void UpdateAnimator(Vector2 direction)
    {
        if (!useAnimator || animator == null)
            return;
        
        try
        {
            animator.SetFloat(moveXParameter, lastDirection.x);
            animator.SetFloat(moveYParameter, lastDirection.y);
            animator.SetBool(isMovingParameter, isMoving);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Ошибка при обновлении Animator: {e.Message}");
        }
    }
    
    /// <summary>
    /// Валидация настроек
    /// </summary>
    private void ValidateSettings()
    {
        // Проверка скорости
        if (speed < minSpeed)
            speed = minSpeed;
        if (speed > maxSpeed)
            speed = maxSpeed;
        
        // Проверка сглаживания
        if (smoothingFactor < 0f)
            smoothingFactor = 0f;
        if (smoothingFactor > 1f)
            smoothingFactor = 1f;
    }
    
    // ===== ПУБЛИЧНЫЕ МЕТОДЫ =====
    
    /// <summary>
    /// Принудительная остановка игрока
    /// </summary>
    public void Stop()
    {
        currentVelocity = Vector2.zero;
        isMoving = false;
        
        if (useAnimator && animator != null)
        {
            animator.SetBool(isMovingParameter, false);
        }
    }
    
    /// <summary>
    /// Установить новую скорость игрока (с валидацией)
    /// </summary>
    public void SetSpeed(float newSpeed)
    {
        speed = Mathf.Clamp(newSpeed, minSpeed, maxSpeed);
    }
    
    /// <summary>
    /// Получить текущую скорость игрока
    /// </summary>
    public float GetSpeed()
    {
        return speed;
    }
    
    /// <summary>
    /// Получить текущее направление движения игрока (нормализованное)
    /// </summary>
    public Vector2 GetCurrentDirection()
    {
        return currentVelocity.normalized;
    }
    
    /// <summary>
    /// Получить последнее направление игрока (для анимации спины и т.д.)
    /// </summary>
    public Vector2 GetLastDirection()
    {
        return lastDirection;
    }
    
    /// <summary>
    /// Получить текущую скорость (не нормализованную)
    /// </summary>
    public Vector2 GetCurrentVelocity()
    {
        return currentVelocity;
    }
    
    /// <summary>
    /// Проверить, движется ли игрок
    /// </summary>
    public bool IsMoving()
    {
        return isMoving;
    }
    
    /// <summary>
    /// Телепортировать игрока на новую позицию
    /// </summary>
    public void Teleport(Vector3 newPosition)
    {
        transform.position = newPosition;
        Stop();
    }
    
    /// <summary>
    /// Телепортировать игрока на новую позицию (Vector2)
    /// </summary>
    public void Teleport(Vector2 newPosition)
    {
        transform.position = newPosition;
        Stop();
    }
    
    /// <summary>
    /// Установить множитель скорости (временный бафф/дебафф)
    /// </summary>
    public void SetSpeedMultiplier(float multiplier)
    {
        speed = Mathf.Clamp(speed * multiplier, minSpeed, maxSpeed);
    }
    
    /// <summary>
    /// Добавить скорость к текущей (для способностей и т.д.)
    /// </summary>
    public void AddSpeed(float speedBonus)
    {
        speed = Mathf.Clamp(speed + speedBonus, minSpeed, maxSpeed);
    }
    
    /// <summary>
    /// Получить минимальную скорость
    /// </summary>
    public float GetMinSpeed()
    {
        return minSpeed;
    }
    
    /// <summary>
    /// Получить максимальную скорость
    /// </summary>
    public float GetMaxSpeed()
    {
        return maxSpeed;
    }
    
    /// <summary>
    /// Установить минимальную скорость
    /// </summary>
    public void SetMinSpeed(float min)
    {
        minSpeed = Mathf.Max(0.1f, min);
        ValidateSettings();
    }
    
    /// <summary>
    /// Установить максимальную скорость
    /// </summary>
    public void SetMaxSpeed(float max)
    {
        maxSpeed = Mathf.Max(0.1f, max);
        ValidateSettings();
    }
    
    /// <summary>
    /// Включить/выключить анимацию
    /// </summary>
    public void SetAnimatorEnabled(bool enabled)
    {
        useAnimator = enabled;
    }
    
    /// <summary>
    /// Включить/выключить сглаживание движения
    /// </summary>
    public void SetSmoothingEnabled(bool enabled)
    {
        useSmoothing = enabled;
    }
    
    /// <summary>
    /// Установить коэффициент сглаживания
    /// </summary>
    public void SetSmoothingFactor(float factor)
    {
        smoothingFactor = Mathf.Clamp01(factor);
    }
    
    /// <summary>
    /// Получить информацию о состоянии игрока (для отладки)
    /// </summary>
    public string GetDebugInfo()
    {
        return $"PlayerMover Debug Info:\n" +
               $"Speed: {speed:F2}\n" +
               $"Current Velocity: {currentVelocity}\n" +
               $"Last Direction: {lastDirection}\n" +
               $"Is Moving: {isMoving}\n" +
               $"Smoothing Enabled: {useSmoothing}";
    }
}
