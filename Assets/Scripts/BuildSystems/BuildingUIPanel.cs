// BuildingUIPanel.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingUIPanel : MonoBehaviour
{
    [SerializeField] private Transform _buttonContainer;
    [SerializeField] private GameObject _buildingButtonPrefab;
    
    private void Start()
    {
        CreateBuildingButtons();
    }

    private void CreateBuildingButtons()
    {
        int buildingCount = BuildingManager.instance.GetBuildingCount();

        for (int i = 0; i < buildingCount; i++)
        {
            BuildingData building = BuildingManager.instance.GetBuilding(i);
            
            GameObject buttonObj = Instantiate(_buildingButtonPrefab, _buttonContainer);
            Button button = buttonObj.GetComponent<Button>();
            Image icon = buttonObj.GetComponent<Image>();
            
            int buildingIndex = i; // Для замыкания в лямбде
            
            if (icon != null)
            {
                icon.sprite = building.buildingIcon;
            }

            button.onClick.AddListener(() => OnBuildingButtonClicked(buildingIndex));

            // Добавляем tooltip
            TextMeshProUGUI text = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = building.buildingName;
            }
        }
    }

    private void OnBuildingButtonClicked(int buildingIndex)
    {
        BuildingManager.instance.StartBuilding(buildingIndex);
    }
}
