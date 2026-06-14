using UnityEngine;

/// <summary>
/// Обработчик ввода для ПК (Windows/Mac/Linux).
/// Использует: WASD/Стрелки, мышь.
/// </summary>
public class PCInputHandler : InputHandler
{
    [Header("Настройки")]
    [SerializeField] private float mouseRayDistance = 10f;
    [SerializeField] private bool debugMode = false;
    
    private Camera mainCamera;
    private Vector3 lastMouseScreenPos;
    
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
            Debug.LogError("PCInputHandler: Нет Camera с тегом 'MainCamera' на сцене!");
            enabled = false;
            return;
        }
        
        if (debugMode)
            Debug.Log("PCInputHandler инициализирован успешно");
    }
    
    /// <summary>
    /// Основной цикл обработки ввода
    /// </summary>
    private void UpdateInput()
    {
        // === 1. Движение (WASD / Стрелки) ===
        HandleMovementInput();
        
        // === 2. Мышь: позиция в мире ===
        HandleMousePositionInput();
        
        // === 3. ЛКМ (первичное действие) ===
        HandlePrimaryInput();
        
        // === 4. ПКМ (вторичное действие) ===
        HandleSecondaryInput();
        
        // === 5. Горячие клавиши ===
        HandleHotkeys();
    }
    
    /// <summary>
    /// Обработка ввода движения (WASD / Стрелки)
    /// </summary>
    private void HandleMovementInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        Vector2 moveDir = new Vector2(horizontal, vertical).normalized;
        InvokeMove(moveDir);
        
        if (debugMode && moveDir.sqrMagnitude > 0)
            Debug.Log($"Направление движения: {moveDir}");
    }
    
    /// <summary>
    /// Обработка позиции мыши в мире
    /// </summary>
    private void HandleMousePositionInput()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        
        // Проверка, что мышь в пределах экрана
        if (!IsMouseInBounds(mouseScreenPos))
            return;
        
        // Конвертируем экранные координаты в мировые
        Vector2 worldMousePos = ScreenToWorldPoint(mouseScreenPos);
        
        InvokeMousePos(worldMousePos);
        
        if (debugMode && mouseScreenPos != lastMouseScreenPos)
            Debug.Log($"Позиция мыши в мире: {worldMousePos}");
        
        lastMouseScreenPos = mouseScreenPos;
    }
    
    /// <summary>
    /// Обработка левой кнопки мыши (первичное действие)
    /// </summary>
    private void HandlePrimaryInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            InvokePrimary();
            if (debugMode)
                Debug.Log("Первичное действие активировано (ЛКМ)");
        }
        
        IsPrimaryHeld = Input.GetMouseButton(0);
    }
    
    /// <summary>
    /// Обработка правой кнопки мыши (вторичное действие)
    /// </summary>
    private void HandleSecondaryInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            InvokeSecondary();
            if (debugMode)
                Debug.Log("Вторичное действие активировано (ПКМ)");
        }
        
        IsSecondaryHeld = Input.GetMouseButton(1);
    }
    
    /// <summary>
    /// Обработка горячих клавиш
    /// </summary>
    private void HandleHotkeys()
    {
        // B - режим строительства
        if (Input.GetKeyDown(KeyCode.B))
        {
            OnBuildMode?.Invoke();
            if (debugMode)
                Debug.Log("Режим строительства активирован (B)");
        }
        
        // Escape - пауза
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPause?.Invoke();
            if (debugMode)
                Debug.Log("Пауза активирована (Escape)");
        }
        
        // Space - альтернативное действие (опционально)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Можете добавить свой обработчик или событие
            if (debugMode)
                Debug.Log("Space нажата");
        }
    }
    
    /// <summary>
    /// Проверка, находится ли мышь в границах экрана
    /// </summary>
    private bool IsMouseInBounds(Vector3 screenPos)
    {
        return screenPos.x >= 0 && screenPos.x <= Screen.width &&
               screenPos.y >= 0 && screenPos.y <= Screen.height;
    }
    
    /// <summary>
    /// Конвертирует экранные координаты в мировые
    /// </summary>
    private Vector2 ScreenToWorldPoint(Vector3 screenPos)
    {
        // Создаём луч от камеры через точку экрана
        Ray ray = mainCamera.ScreenPointToRay(screenPos);
        
        // Вычисляем точку в мире на определённой дистанции
        Vector3 worldPoint = ray.origin + ray.direction * mouseRayDistance;
        
        return new Vector2(worldPoint.x, worldPoint.y);
    }
    
    /// <summary>
    /// Получить позицию мыши в экранных координатах
    /// </summary>
    public Vector3 GetMouseScreenPosition()
    {
        return Input.mousePosition;
    }
    
    /// <summary>
    /// Установить дистанцию для ray-casting мыши
    /// </summary>
    public void SetMouseRayDistance(float distance)
    {
        mouseRayDistance = Mathf.Max(0.1f, distance);
    }
    
    /// <summary>
    /// Получить текущую дистанцию для ray-casting
    /// </summary>
    public float GetMouseRayDistance()
    {
        return mouseRayDistance;
    }
    
    /// <summary>
    /// Включить/выключить режим отладки
    /// </summary>
    public void SetDebugMode(bool enabled)
    {
        debugMode = enabled;
    }
    
    /// <summary>
    /// Проверить, нажата ли определённая клавиша
    /// </summary>
    public bool IsKeyPressed(KeyCode key)
    {
        return Input.GetKey(key);
    }
    
    /// <summary>
    /// Проверить, нажата ли определённая клавиша в этом кадре
    /// </summary>
    public bool IsKeyPressedThisFrame(KeyCode key)
    {
        return Input.GetKeyDown(key);
    }
}