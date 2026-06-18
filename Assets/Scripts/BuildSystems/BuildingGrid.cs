// BuildingGrid.cs
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    [Header("Размер и позиция сетки")]
    [SerializeField] private int _gridWidth = 10;
    [SerializeField] private int _gridHeight = 10;
    [SerializeField] private float _cellSize = 1f;
    [SerializeField] private Vector3 _gridStartPosition = Vector3.zero;
    
    [Header("Префаб клетки")]
    [SerializeField] private GameObject _cellPrefab;
    
    [Header("Визуализация")]
    [SerializeField] private bool _showGridGizmos = true;
    [SerializeField] private Color _gizmoColor = Color.green;
    
    private GridCell[,] _grid;
    private bool _gridActive = false;
    
    // Кэш для быстрого поиска
    private Transform _gridParent;

    public static BuildingGrid instance;

    private void Awake()
    {
        if (instance == null) 
            instance = this;
    }

    private void Start()
    {
        CreateGrid();
        HideGrid();
    }

    private void CreateGrid()
    {
        _grid = new GridCell[_gridWidth, _gridHeight];
        
        // Создаём родительский объект для всех ячеек
        if (_gridParent == null)
        {
            GameObject gridParentObj = new GameObject("GridCells");
            gridParentObj.transform.parent = transform;
            gridParentObj.transform.localPosition = Vector3.zero;
            _gridParent = gridParentObj.transform;
        }

        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                // Вычисляем позицию с центровкой
                Vector3 cellPosition = GetCellWorldPosition(x, y);
                
                GameObject cellObj = Instantiate(_cellPrefab, cellPosition, Quaternion.identity, _gridParent);
                cellObj.name = $"Cell_{x}_{y}";

                GridCell cell = cellObj.GetComponent<GridCell>();
                if (cell != null)
                {
                    cell.Initialize(x, y);
                }

                _grid[x, y] = cell;
            }
        }
        
        Debug.Log($"Сетка создана: {_gridWidth}x{_gridHeight}, начало: {_gridStartPosition}");
    }

    public void ShowGrid()
    {
        _gridActive = true;
        _gridParent.gameObject.SetActive(true);
        Debug.Log("Сетка видна");
    }

    public void HideGrid()
    {
        _gridActive = false;
        _gridParent.gameObject.SetActive(false);
        Debug.Log("Сетка скрыта");
    }

    public bool IsGridActive()
    {
        return _gridActive;
    }

    /// <summary>
    /// Получает мировую позицию центра ячейки по координатам сетки
    /// </summary>
    public Vector3 GetCellWorldPosition(int x, int y)
    {
        // Центр ячейки находится на расстоянии cellSize/2 от левого нижнего угла
        float worldX = _gridStartPosition.x + (x * _cellSize) + (_cellSize / 2f);
        float worldY = _gridStartPosition.y + (y * _cellSize) + (_cellSize / 2f);
        
        return new Vector3(worldX, worldY, 0);
    }

    /// <summary>
    /// Находит ячейку по мировой позиции
    /// </summary>
    public GridCell GetCellAtWorldPosition(Vector3 worldPosition)
    {
        // Вычитаем стартовую позицию и делим на размер ячейки
        float relativeX = worldPosition.x - _gridStartPosition.x;
        float relativeY = worldPosition.y - _gridStartPosition.y;

        int x = Mathf.FloorToInt(relativeX / _cellSize);
        int y = Mathf.FloorToInt(relativeY / _cellSize);

        Debug.Log($"Позиция мыши: {worldPosition}, Относительные координаты: ({relativeX}, {relativeY}), GridPos: ({x}, {y})");

        if (IsValidGridPosition(x, y))
        {
            Debug.Log($"Ячейка найдена: ({x}, {y})");
            return _grid[x, y];
        }
        else
        {
            Debug.LogWarning($"Ячейка ({x}, {y}) вне границ сетки!");
        }

        return null;
    }

    /// <summary>
    /// Получает ячейку по координатам
    /// </summary>
    public GridCell GetCellAtGridPosition(int x, int y)
    {
        if (IsValidGridPosition(x, y))
        {
            return _grid[x, y];
        }

        return null;
    }

    /// <summary>
    /// Проверяет валидность координат
    /// </summary>
    private bool IsValidGridPosition(int x, int y)
    {
        return x >= 0 && x < _gridWidth && y >= 0 && y < _gridHeight;
    }

    public void OccupyCell(int x, int y)
    {
        GridCell cell = GetCellAtGridPosition(x, y);
        if (cell != null)
        {
            cell.Occupy();
        }
    }

    public void FreeCell(int x, int y)
    {
        GridCell cell = GetCellAtGridPosition(x, y);
        if (cell != null)
        {
            cell.Free();
        }
    }

    public bool IsCellAvailable(int x, int y)
    {
        GridCell cell = GetCellAtGridPosition(x, y);
        return cell != null && !cell.IsOccupied;
    }

    public float GetCellSize()
    {
        return _cellSize;
    }

    public Vector3 GetGridStartPosition()
    {
        return _gridStartPosition;
    }

    public int GetGridWidth()
    {
        return _gridWidth;
    }

    public int GetGridHeight()
    {
        return _gridHeight;
    }

    public void ResetGrid()
    {
        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                _grid[x, y].Free();
            }
        }
    }

    /// <summary>
    /// Визуализация сетки в редакторе
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (!_showGridGizmos) return;

        Gizmos.color = _gizmoColor;

        // Рисуем горизонтальные линии
        for (int y = 0; y <= _gridHeight; y++)
        {
            Vector3 startPos = _gridStartPosition + new Vector3(0, y * _cellSize, 0);
            Vector3 endPos = startPos + new Vector3(_gridWidth * _cellSize, 0, 0);
            Gizmos.DrawLine(startPos, endPos);
        }

        // Рисуем вертикальные линии
        for (int x = 0; x <= _gridWidth; x++)
        {
            Vector3 startPos = _gridStartPosition + new Vector3(x * _cellSize, 0, 0);
            Vector3 endPos = startPos + new Vector3(0, _gridHeight * _cellSize, 0);
            Gizmos.DrawLine(startPos, endPos);
        }

        // Рисуем стартовую позицию
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_gridStartPosition, Vector3.one * 0.2f);
    }
}
