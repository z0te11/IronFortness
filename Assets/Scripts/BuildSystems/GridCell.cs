// GridCell.cs
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Color _availableColor = Color.green;
    [SerializeField] private Color _unavailableColor = Color.red;
    [SerializeField] private Color _defaultColor = Color.white;
    
    private bool _isOccupied = false;
    private int _gridX;
    private int _gridY;

    public bool IsOccupied => _isOccupied;
    public int GridX => _gridX;
    public int GridY => _gridY;

    private void Start()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    public void Initialize(int x, int y)
    {
        _gridX = x;
        _gridY = y;
        _isOccupied = false;
        SetState(CellState.Available);
    }

    public void SetState(CellState state)
    {
        switch (state)
        {
            case CellState.Available:
                _spriteRenderer.color = _availableColor;
                break;
            case CellState.Unavailable:
                _spriteRenderer.color = _unavailableColor;
                break;
            case CellState.Default:
                _spriteRenderer.color = _defaultColor;
                break;
        }
    }

    public void Occupy()
    {
        _isOccupied = true;
        SetState(CellState.Default);
    }

    public void Free()
    {
        _isOccupied = false;
        SetState(CellState.Available);
    }
}

public enum CellState
{
    Available,
    Unavailable,
    Default
}
