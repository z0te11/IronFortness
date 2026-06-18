// BuildingData.cs
using UnityEngine;

[System.Serializable]
public class BuildingData
{
    public string buildingName;
    public GameObject buildingPrefab;
    public Sprite buildingIcon;
    [TextArea]
    public string description;
    
    [Header("Стоимость")]
    public int cost;
    
    [Header("Размер")]
    public int width = 1;
    public int height = 1;
}
