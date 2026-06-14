using UnityEngine;

public abstract class InputHandler : MonoBehaviour
{
    // === События (для подписчиков) ===
    public System.Action<Vector2> OnMoveInput;           // Направление движения (WASD/стик)
    public System.Action<Vector2> OnMousePosition;       // Позиция мыши/пальца в мире
    public System.Action OnPrimaryAction;                // ЛКМ / Tap
    public System.Action OnSecondaryAction;              // ПКМ / Long tap
    public System.Action OnBuildMode;                    // Клавиша B / кнопка на UI
    public System.Action OnPause;                        // Escape / кнопка паузы
    
    public Vector2 MoveDirection { get; protected set; } = Vector2.zero;
    public Vector2 MouseWorldPosition { get; protected set; } = Vector2.zero;
    public bool IsPrimaryHeld { get; protected set; } = false;
    public bool IsSecondaryHeld { get; protected set; } = false;
    
    public bool InputEnabled { get; set; } = true;
    
    public virtual void Initialize() { }
    
    public Vector2 GetRawMoveDirection() => MoveDirection;
    
    public bool IsMoving() => MoveDirection.sqrMagnitude > 0.01f;
    
    public void SetInputEnabled(bool enabled)
    {
        InputEnabled = enabled;
        if (!enabled)
        {
            // При отключении очищаем состояния
            MoveDirection = Vector2.zero;
            IsPrimaryHeld = false;
            IsSecondaryHeld = false;
            OnMoveInput?.Invoke(Vector2.zero);
        }
    }
    
    /// <summary>
    /// Принудительно вызвать событие движения
    /// </summary>
    public void ForceMove(Vector2 direction)
    {
        InvokeMove(direction);
    }
    
    /// <summary>
    /// Принудительно вызвать событие позиции мыши
    /// </summary>
    public void ForceMousePos(Vector2 position)
    {
        InvokeMousePos(position);
    }
    
    /// <summary>
    /// Очистить все состояния ввода
    /// </summary>
    public void ClearInputState()
    {
        MoveDirection = Vector2.zero;
        MouseWorldPosition = Vector2.zero;
        IsPrimaryHeld = false;
        IsSecondaryHeld = false;
    }
    
    // ===== Защищённые методы для вызова событий =====
    
    /// <summary>
    /// Внутренний метод для вызова события движения
    /// </summary>
    protected void InvokeMove(Vector2 dir)
    {
        if (!InputEnabled) 
            return;
        
        MoveDirection = dir;
        OnMoveInput?.Invoke(dir);
    }
    
    /// <summary>
    /// Внутренний метод для вызова события позиции мыши
    /// </summary>
    protected void InvokeMousePos(Vector2 pos)
    {
        if (!InputEnabled)
            return;
        
        MouseWorldPosition = pos;
        OnMousePosition?.Invoke(pos);
    }
    
    /// <summary>
    /// Внутренний метод для вызова первичного действия
    /// </summary>
    protected void InvokePrimary()
    {
        if (!InputEnabled)
            return;
        
        OnPrimaryAction?.Invoke();
    }
    
    /// <summary>
    /// Внутренний метод для вызова вторичного действия
    /// </summary>
    protected void InvokeSecondary()
    {
        if (!InputEnabled)
            return;
        
        OnSecondaryAction?.Invoke();
    }
    
    /// <summary>
    /// Внутренний метод для вызова режима строительства
    /// </summary>
    protected void InvokeBuildMode()
    {
        if (!InputEnabled)
            return;
        
        OnBuildMode?.Invoke();
    }
    
    /// <summary>
    /// Внутренний метод для вызова паузы
    /// </summary>
    protected void InvokePause()
    {
        if (!InputEnabled)
            return;
        
        OnPause?.Invoke();
    }
}
