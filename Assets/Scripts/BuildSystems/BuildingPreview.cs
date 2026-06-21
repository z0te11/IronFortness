// BuildingPreview.cs
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Color _validColor = new Color(0, 1, 0, 0.5f);
    private Color _invalidColor = new Color(1, 0, 0, 0.5f);
    private Color _originalColor;
    
    private GridCell _currentCell;
    private bool _canPlace = false;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (_spriteRenderer == null)
        {
            Debug.LogError("BuildingPreview не имеет SpriteRenderer!");
        }
        else
        {
            _originalColor = _spriteRenderer.color;
        }
        
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
        if (BuildingGrid.instance == null) return;

        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = 0f;
        
        _currentCell = BuildingGrid.instance.GetCellAtWorldPosition(worldPos);

        if (_currentCell != null)
        {
            // Получаем позицию ячейки сетки напрямую
            GridCell gridCell = _currentCell;
            Vector3 cellWorldPos = BuildingGrid.instance.GetCellWorldPosition(gridCell.GridX, gridCell.GridY);
            
            cellWorldPos.z = -1f;
            transform.position = cellWorldPos;
            
            Debug.Log($"Превью на ячейке: ({gridCell.GridX}, {gridCell.GridY}), позиция: {cellWorldPos}");
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
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (_spriteRenderer != null && sprite != null)
        {
            _spriteRenderer.sprite = sprite;
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
