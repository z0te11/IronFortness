using UnityEngine;

/// <summary>
/// Обработчик ввода для мобильных устройств (iOS/Android).
/// Использует: тачскрин, жесты, ускорометр (опционально).
/// </summary>
public class MobileInputHandler : InputHandler
{
    [Header("Настройки тачскрина")]
    [SerializeField] private float swipeThreshold = 50f;
    [SerializeField] private float tapDuration = 0.3f;
    [SerializeField] private float longTapDuration = 0.5f;
    [SerializeField] private float mouseRayDistance = 10f;
    [SerializeField] private bool debugMode = false;
    
    [Header("Джойстик (опционально)")]
    [SerializeField] private bool useVirtualJoystick = false;
    [SerializeField] private float joystickSize = 100f;
    [SerializeField] private float joystickDeadZone = 0.2f;
    
    [Header("Акселерометр (опционально)")]
    [SerializeField] private bool useAccelerometer = false;
    [SerializeField] private float accelerometerSensitivity = 0.5f;
    
    private Camera mainCamera;
    private Touch currentTouch;
    private Vector2 touchStartPos;
    private Vector2 touchCurrentPos;
    private float touchStartTime;
    private bool isTouching = false;
    private Vector2 lastDirection = Vector2.zero;
    
    // Виртуальный джойстик
    private Vector2 joystickCenter;
    private bool joystickActive = false;
    
    // Акселерометр
    private Vector2 accelerometerInput = Vector2.zero;
    
    private void Start()
    {
        Initialize();
    }
    
    private void Update()
    {
        if (!InputEnabled || mainCamera == null)
            return;
        
        UpdateInput();
    }
    
    /// <summary>
    /// Инициализация компонента
    /// </summary>
    public override void Initialize()
    {
        mainCamera = Camera.main;
        
        if (mainCamera == null)
        {
            Debug.LogError("MobileInputHandler: Нет Camera с тегом 'MainCamera' на сцене!");
            enabled = false;
            return;
        }
        
        // Инициализация акселерометра если включен
        if (useAccelerometer)
        {
            Input.gyro.enabled = true;
        }
        
        // Инициализация виртуального джойстика
        if (useVirtualJoystick)
        {
            joystickCenter = new Vector2(joystickSize + 10, Screen.height - joystickSize - 10);
        }
        
        if (debugMode)
            Debug.Log("MobileInputHandler инициализирован успешно");
    }
    
    /// <summary>
    /// Основной цикл обработки ввода
    /// </summary>
    private void UpdateInput()
    {
        // === 1. Обработка тачскрина ===
        HandleTouchInput();
        
        // === 2. Обработка акселерометра ===
        if (useAccelerometer)
            HandleAccelerometerInput();
        
        // === 3. Обработка виртуального джойстика ===
        if (useVirtualJoystick)
            HandleVirtualJoystick();
        
        // === 4. Обработка гравитационного ввода ===
        if (useAccelerometer && !useVirtualJoystick)
            ApplyAccelerometerMovement();
    }
    
    /// <summary>
    /// Обработка ввода с тачскрина
    /// </summary>
    private void HandleTouchInput()
    {
        if (Input.touchCount == 0)
        {
            // Палец отпущен
            if (isTouching)
            {
                isTouching = false;
                InvokeMove(Vector2.zero);
                
                if (debugMode)
                    Debug.Log("Палец отпущен");
            }
            return;
        }
        
        currentTouch = Input.GetTouch(0);
        touchCurrentPos = currentTouch.position;
        
        switch (currentTouch.phase)
        {
            case TouchPhase.Began:
                HandleTouchBegan();
                break;
            
            case TouchPhase.Moved:
                HandleTouchMoved();
                break;
            
            case TouchPhase.Ended:
                HandleTouchEnded();
                break;
            
            case TouchPhase.Canceled:
                HandleTouchCanceled();
                break;
        }
    }
    
    /// <summary>
    /// Обработка начала касания
    /// </summary>
    private void HandleTouchBegan()
    {
        isTouching = true;
        touchStartPos = currentTouch.position;
        touchStartTime = Time.time;
        
        // Обновляем позицию мыши
        Vector2 worldMousePos = ScreenToWorldPoint(touchStartPos);
        InvokeMousePos(worldMousePos);
        
        if (debugMode)
            Debug.Log($"Касание началось в позиции: {touchStartPos}");
    }
    
    /// <summary>
    /// Обработка движения касания (свайп)
    /// </summary>
    private void HandleTouchMoved()
    {
        if (!isTouching)
            return;
        
        // Обновляем позицию мыши
        Vector2 worldMousePos = ScreenToWorldPoint(touchCurrentPos);
        InvokeMousePos(worldMousePos);
        
        // Вычисляем направление свайпа
        Vector2 swipeDelta = touchCurrentPos - touchStartPos;
        
        if (swipeDelta.sqrMagnitude > swipeThreshold * swipeThreshold)
        {
            // Свайп достаточно длинный для обработки как движения
            Vector2 swipeDirection = swipeDelta.normalized;
            InvokeMove(swipeDirection);
            
            if (debugMode)
                Debug.Log($"Свайп в направлении: {swipeDirection}");
        }
    }
    
    /// <summary>
    /// Обработка конца касания
    /// </summary>
    private void HandleTouchEnded()
    {
        isTouching = false;
        float touchDuration = Time.time - touchStartTime;
        
        Vector2 swipeDelta = touchCurrentPos - touchStartPos;
        
        // Проверяем тип касания
        if (swipeDelta.sqrMagnitude < swipeThreshold * swipeThreshold)
        {
            // Это был тап (короткое касание без движения)
            if (touchDuration < tapDuration)
            {
                InvokePrimary();
                if (debugMode)
                    Debug.Log("Первичное действие (тап)");
            }
            // Долгое касание
            else if (touchDuration > longTapDuration)
            {
                InvokeSecondary();
                if (debugMode)
                    Debug.Log("Вторичное действие (долгое касание)");
            }
        }
        
        InvokeMove(Vector2.zero);
    }
    
    /// <summary>
    /// Обработка отмены касания (прерывание)
    /// </summary>
    private void HandleTouchCanceled()
    {
        isTouching = false;
        InvokeMove(Vector2.zero);
        
        if (debugMode)
            Debug.Log("Касание отменено");
    }
    
    /// <summary>
    /// Обработка ввода акселерометра
    /// </summary>
    private void HandleAccelerometerInput()
    {
        // Получаем данные акселерометра
        Vector3 acceleration = Input.acceleration;
        
        // Конвертируем в направление движения
        accelerometerInput = new Vector2(acceleration.x, acceleration.y).normalized;
        
        // Применяем чувствительность
        accelerometerInput *= accelerometerSensitivity;
        
        if (debugMode && accelerometerInput.sqrMagnitude > 0)
            Debug.Log($"Акселерометр: {accelerometerInput}");
    }
    
    /// <summary>
    /// Обработка виртуального джойстика
    /// </summary>
    private void HandleVirtualJoystick()
    {
        if (Input.touchCount == 0)
        {
            joystickActive = false;
            InvokeMove(Vector2.zero);
            return;
        }
        
        currentTouch = Input.GetTouch(0);
        Vector2 touchPos = currentTouch.position;
        
        // Проверяем, в пределах ли джойстика
        float distToCenter = Vector2.Distance(touchPos, joystickCenter);
        
        if (distToCenter <= joystickSize)
        {
            joystickActive = true;
            
            // Вычисляем направление от центра джойстика до касания
            Vector2 joystickDirection = (touchPos - joystickCenter).normalized;
            
            // Применяем мёртвую зону
            if (distToCenter < joystickSize * joystickDeadZone)
            {
                joystickDirection = Vector2.zero;
            }
            
            InvokeMove(joystickDirection);
            
            if (debugMode && joystickDirection.sqrMagnitude > 0)
                Debug.Log($"Джойстик направление: {joystickDirection}");
        }
        else
        {
            joystickActive = false;
            InvokeMove(Vector2.zero);
        }
    }
    
    /// <summary>
    /// Применение ввода акселерометра для движения
    /// </summary>
    private void ApplyAccelerometerMovement()
    {
        if (accelerometerInput.sqrMagnitude > joystickDeadZone)
        {
            InvokeMove(accelerometerInput);
        }
        else
        {
            InvokeMove(Vector2.zero);
        }
    }
    
    /// <summary>
    /// Конвертирует экранные координаты в мировые
    /// </summary>
    private Vector2 ScreenToWorldPoint(Vector3 screenPos)
    {
        Ray ray = mainCamera.ScreenPointToRay(screenPos);
        Vector3 worldPoint = ray.origin + ray.direction * mouseRayDistance;
        return new Vector2(worldPoint.x, worldPoint.y);
    }
    
    /// <summary>
    /// Проверить, используется ли виртуальный джойстик
    /// </summary>
    public bool IsVirtualJoystickActive()
    {
        return joystickActive && useVirtualJoystick;
    }
    
    /// <summary>
    /// Установить размер виртуального джойстика
    /// </summary>
    public void SetJoystickSize(float size)
    {
        joystickSize = Mathf.Max(10f, size);
    }
    
    /// <summary>
    /// Установить позицию виртуального джойстика
    /// </summary>
    public void SetJoystickPosition(Vector2 position)
    {
        joystickCenter = position;
    }
    
    /// <summary>
    /// Установить мёртвую зону джойстика (0-1)
    /// </summary>
    public void SetJoystickDeadZone(float deadZone)
    {
        joystickDeadZone = Mathf.Clamp01(deadZone);
    }
    
    /// <summary>
    /// Установить чувствительность акселерометра
    /// </summary>
    public void SetAccelerometerSensitivity(float sensitivity)
    {
        accelerometerSensitivity = Mathf.Max(0.1f, sensitivity);
    }
    
    /// <summary>
    /// Установить порог свайпа
    /// </summary>
    public void SetSwipeThreshold(float threshold)
    {
        swipeThreshold = Mathf.Max(10f, threshold);
    }
    
    /// <summary>
    /// Включить/выключить виртуальный джойстик
    /// </summary>
    public void SetVirtualJoystickEnabled(bool enabled)
    {
        useVirtualJoystick = enabled;
        if (enabled)
            joystickCenter = new Vector2(joystickSize + 10, Screen.height - joystickSize - 10);
    }
    
    /// <summary>
    /// Включить/выключить акселерометр
    /// </summary>
    public void SetAccelerometerEnabled(bool enabled)
    {
        useAccelerometer = enabled;
        if (enabled)
            Input.gyro.enabled = true;
        else
            Input.gyro.enabled = false;
    }
    
    /// <summary>
    /// Включить/выключить режим отладки
    /// </summary>
    public void SetDebugMode(bool enabled)
    {
        debugMode = enabled;
    }
    
    /// <summary>
    /// Получить информацию о текущем состоянии (для отладки)
    /// </summary>
    public string GetDebugInfo()
    {
        return $"MobileInputHandler Debug Info:\n" +
               $"Touch Count: {Input.touchCount}\n" +
               $"Is Touching: {isTouching}\n" +
               $"Touch Start Pos: {touchStartPos}\n" +
               $"Touch Current Pos: {touchCurrentPos}\n" +
               $"Virtual Joystick Active: {joystickActive}\n" +
               $"Accelerometer Input: {accelerometerInput}\n" +
               $"Last Direction: {lastDirection}";
    }
    
    /// <summary>
    /// Нарисовать виртуальный джойстик в OnGUI (для визуализации)
    /// </summary>
    private void OnGUI()
    {
        if (!useVirtualJoystick || !debugMode)
            return;
        
        // Рисуем круг джойстика
        GUI.color = joystickActive ? Color.green : Color.white;
        GUI.Box(new Rect(joystickCenter.x - joystickSize, 
                         Screen.height - joystickCenter.y - joystickSize, 
                         joystickSize * 2, 
                         joystickSize * 2), "");
        
        GUI.color = Color.white;
    }
}
