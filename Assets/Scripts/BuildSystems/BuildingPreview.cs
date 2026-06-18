// BuildingPreview.cs
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Color _validColor = new Color(0, 1, 0, 0.5f);
    private Color _invalidColor = new Color(1, 0, 0, 0.5f);
    
    private GridCell _currentCell;
    private bool _canPlace = false;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!gameObject.activeSelf) return;

        UpdatePosition();
        CheckPlacement();
    }

    private void UpdatePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        
        // Правильное преобразование координат мыши в мировые координаты
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        
        // Обнуляем z-координату (нам нужна только x и y)
        worldPos.z = 0f;
        
        _currentCell = BuildingGrid.instance.GetCellAtWorldPosition(worldPos);

        if (_currentCell != null)
        {
            Vector3 cellCenterPos = BuildingGrid.instance.GetCellWorldPosition(_currentCell.GridX, _currentCell.GridY);

            
            cellCenterPos.z = -1f;
            transform.position = cellCenterPos;
        }
    }

    private void CheckPlacement()
    {
        if (_currentCell == null)
        {
            _canPlace = false;
            if (_spriteRenderer != null)
                _spriteRenderer.color = _invalidColor;
            return;
        }

        _canPlace = !_currentCell.IsOccupied;
        if (_spriteRenderer != null)
            _spriteRenderer.color = _canPlace ? _validColor : _invalidColor;
    }

    public bool CanPlace()
    {
        return _canPlace && _currentCell != null;
    }

    public GridCell GetCurrentCell()
    {
        return _currentCell;
    }

    public void SetPreviewSprite(Sprite sprite)
    {

        if (_spriteRenderer != null && sprite != null)
        {
            _spriteRenderer.sprite = sprite;
        }
        else
        {
            Debug.LogError($"Не удалось установить спрайт: SpriteRenderer={_spriteRenderer}, Sprite={sprite}");
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
