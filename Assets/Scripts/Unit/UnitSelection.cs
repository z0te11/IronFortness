// UnitSelection.cs
using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    [SerializeField] private InputHandler _inputHandler;
    
    private SelectableUnit _selectedUnit;
    private Camera _mainCamera;

    private void Start()
    {
        if (_inputHandler == null)
        {
            _inputHandler = GetComponent<InputHandler>();
        }

        _mainCamera = Camera.main;

        if (_inputHandler != null)
        {
            _inputHandler.OnPrimaryAction += HandlePrimaryAction;
            _inputHandler.OnMousePosition += HandleMousePosition;
        }
    }

    private void OnDestroy()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnPrimaryAction -= HandlePrimaryAction;
            _inputHandler.OnMousePosition -= HandleMousePosition;
        }
    }

    /// <summary>
    /// Обработка клика (первичное действие)
    /// </summary>
    private void HandlePrimaryAction()
    {
        Vector2 mouseWorldPos = _inputHandler.MouseWorldPosition;

        // Пытаемся выбрать юнита
        SelectableUnit clickedUnit = GetUnitAtPosition(mouseWorldPos);

        if (clickedUnit != null)
        {
            // Кликнули на юнита - выбираем его
            SelectUnit(clickedUnit);
        }
        else if (_selectedUnit != null)
        {
            // Кликнули на пустое место - приказываем идти
            _selectedUnit.OrderMove(new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0));
        }
    }

    /// <summary>
    /// Обработка позиции мыши (для визуального курсора)
    /// </summary>
    private void HandleMousePosition(Vector2 mousePos)
    {
        // Можно добавить визуализацию курсора здесь
    }

    /// <summary>
    /// Выбрать юнита
    /// </summary>
    private void SelectUnit(SelectableUnit unit)
    {
        // Деселект предыдущего
        if (_selectedUnit != null && _selectedUnit != unit)
        {
            _selectedUnit.Deselect();
        }

        // Выбираем нового
        _selectedUnit = unit;
        _selectedUnit.Select();
    }

    /// <summary>
    /// Найти юнита в позиции
    /// </summary>
    private SelectableUnit GetUnitAtPosition(Vector2 position)
    {
        // Используем коллайдер для проверки
        Collider2D[] colliders = Physics2D.OverlapPointAll(position);

        foreach (Collider2D collider in colliders)
        {
            SelectableUnit unit = collider.GetComponent<SelectableUnit>();
            if (unit != null)
            {
                return unit;
            }
        }

        return null;
    }

    /// <summary>
    /// Получить выбранного юнита
    /// </summary>
    public SelectableUnit GetSelectedUnit()
    {
        return _selectedUnit;
    }

    /// <summary>
    /// Деселект текущего юнита
    /// </summary>
    public void DeselectCurrent()
    {
        if (_selectedUnit != null)
        {
            _selectedUnit.Deselect();
            _selectedUnit = null;
        }
    }
}
