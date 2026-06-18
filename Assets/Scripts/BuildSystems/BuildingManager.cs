// BuildingManager.cs
using UnityEngine;
using System.Collections.Generic;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private BuildingData[] _availableBuildings;
    [SerializeField] private BuildingPreview _buildingPreviewPrefab;
    
    private BuildingPreview _currentPreview;
    private BuildingData _selectedBuilding;
    private bool _isBuilding = false;
    
    private List<PlacedBuilding> _placedBuildings = new List<PlacedBuilding>();

    public static BuildingManager instance;

    private void Awake()
    {
        if (instance == null) 
            instance = this;
    }

    private void Start()
    {

        _currentPreview = Instantiate(_buildingPreviewPrefab);
        _currentPreview.Hide();
        
        Debug.Log("BuildingManager инициализирован");
    }

    private void Update()
    {
        if (_isBuilding && _currentPreview != null && _currentPreview.gameObject.activeSelf)
        {
            HandleBuildingInput();
        }
    }

    private void HandleBuildingInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_currentPreview.CanPlace())
            {
                PlaceBuilding();
            }
            else
            {
                Debug.LogWarning("Нельзя построить здание здесь!");
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelBuilding();
        }
    }

    public void StartBuilding(int buildingIndex)
    {
        if (_currentPreview == null)
        {
            Debug.LogError("BuildingPreview не инициализирован!");
            return;
        }

        if (buildingIndex >= _availableBuildings.Length)
        {
            Debug.LogError("Здание не найдено!");
            return;
        }

        _selectedBuilding = _availableBuildings[buildingIndex];

        if (!MoneyManager.instance.CheckIsHaveMoney(_selectedBuilding.cost))
        {
            Debug.LogWarning("Недостаточно денег!");
            return;
        }

        _isBuilding = true;
        
        BuildingGrid.instance.ShowGrid();
        
        _currentPreview.SetPreviewSprite(_selectedBuilding.buildingIcon);
        
        _currentPreview.Show();

        Debug.Log($"Начата постройка: {_selectedBuilding.buildingName}");
    }

    private void PlaceBuilding()
    {
        GridCell cell = _currentPreview.GetCurrentCell();

        if (cell == null) 
        {
            Debug.LogWarning("Ячейка не найдена!");
            return;
        }

        MoneyManager.instance.SpendMoney(_selectedBuilding.cost);

        Vector3 buildingPosition = BuildingGrid.instance.GetCellWorldPosition(cell.GridX, cell.GridY);
        GameObject newBuilding = Instantiate(_selectedBuilding.buildingPrefab, buildingPosition, Quaternion.identity);

        BuildingGrid.instance.OccupyCell(cell.GridX, cell.GridY);

        PlacedBuilding placedBuilding = new PlacedBuilding
        {
            buildingObject = newBuilding,
            buildingData = _selectedBuilding,
            gridX = cell.GridX,
            gridY = cell.GridY
        };
        _placedBuildings.Add(placedBuilding);

        Debug.Log($"Здание '{_selectedBuilding.buildingName}' построено на позиции ({cell.GridX}, {cell.GridY})");

        CancelBuilding();
    }

    public void CancelBuilding()
    {
        _isBuilding = false;
        _selectedBuilding = null;
        _currentPreview.Hide();
        BuildingGrid.instance.HideGrid();
        Debug.Log("Строительство отменено");
    }

    public BuildingData GetBuilding(int index)
    {
        if (index >= 0 && index < _availableBuildings.Length)
        {
            return _availableBuildings[index];
        }
        return null;
    }

    public int GetBuildingCount()
    {
        return _availableBuildings.Length;
    }

    public bool IsBuilding()
    {
        return _isBuilding;
    }

    public List<PlacedBuilding> GetPlacedBuildings()
    {
        return new List<PlacedBuilding>(_placedBuildings);
    }
}

[System.Serializable]
public class PlacedBuilding
{
    public GameObject buildingObject;
    public BuildingData buildingData;
    public int gridX;
    public int gridY;
}
