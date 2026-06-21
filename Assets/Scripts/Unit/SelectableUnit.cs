// SelectableUnit.cs
using UnityEngine;

public class SelectableUnit : MonoBehaviour
{
    [Header("Визуализация выделения")]
    [SerializeField] private GameObject _selectionIndicator;
    [SerializeField] private Color _selectedColor = Color.green;
    [SerializeField] private Color _normalColor = Color.white;
    
    private SpriteRenderer _spriteRenderer;
    private bool _isSelected = false;
    private UnitMovement _unitMovement;
    private UnitAttack _unitAttack;

    public bool IsSelected => _isSelected;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _unitMovement = GetComponent<UnitMovement>();
        _unitAttack = GetComponent<UnitAttack>();

        if (_selectionIndicator != null)
        {
            _selectionIndicator.SetActive(false);
        }
    }

    /// <summary>
    /// Выбрать юнита
    /// </summary>
    public void Select()
    {
        if (_isSelected) return;

        _isSelected = true;

        if (_selectionIndicator != null)
        {
            _selectionIndicator.SetActive(true);
        }

        if (_spriteRenderer != null)
        {
            _spriteRenderer.color = _selectedColor;
        }

        Debug.Log($"Юнит '{gameObject.name}' выбран");
    }

    /// <summary>
    /// Девыбрать юнита
    /// </summary>
    public void Deselect()
    {
        if (!_isSelected) return;

        _isSelected = false;

        if (_selectionIndicator != null)
        {
            _selectionIndicator.SetActive(false);
        }

        if (_spriteRenderer != null)
        {
            _spriteRenderer.color = _normalColor;
        }

        Debug.Log($"Юнит '{gameObject.name}' отменен выбор");
    }

    /// <summary>
    /// Приказать идти на позицию
    /// </summary>
    public void OrderMove(Vector3 targetPosition)
    {
        if (!_isSelected) return;

        if (_unitMovement != null)
        {
            _unitMovement.MoveTo(targetPosition);
            Debug.Log($"Приказ: идти на позицию {targetPosition}");
        }
    }

    /// <summary>
    /// Приказать атаковать юнита
    /// </summary>
    public void OrderAttack(SelectableUnit targetUnit)
    {
        if (!_isSelected) return;

        if (_unitAttack != null && targetUnit != null)
        {
            _unitAttack.AttackUnit(targetUnit.gameObject);
            Debug.Log($"Приказ: атаковать {targetUnit.gameObject.name}");
        }
    }

    /// <summary>
    /// Проверить, находится ли позиция в пределах кликабельности
    /// </summary>
    public bool IsClickInBounds(Vector3 clickPosition)
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null) return false;

        return collider.OverlapPoint(clickPosition);
    }
}
