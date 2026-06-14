// PlayerPool.cs
using System.Collections.Generic;
using UnityEngine;

public class PlayerPool : MonoBehaviour
{
    [SerializeField] private GameObject _playerMainBuild;
    private List<GameObject> _playerUnits = new List<GameObject>();

    public static PlayerPool instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public GameObject GetMainBuildPlayer()
    {
        if (_playerMainBuild != null) return _playerMainBuild;
        return null;
    }

    public void SetMainBuildPlayer(GameObject mainBuilding)
    {
        _playerMainBuild = mainBuilding;
    }

    public void AddUnitToPool(GameObject newUnit)
    {
        if (!_playerUnits.Contains(newUnit))
        {
            _playerUnits.Add(newUnit);
        }
    }

    public void RemoveUnitFromPool(GameObject newUnit)
    {
        _playerUnits.Remove(newUnit);
    }

    public GameObject FindNearestPlayerUnit(Vector3 origin, float radius)
    {
        float closestDistance = radius;
        GameObject nearestUnit = null;

        foreach (GameObject unit in _playerUnits)
        {
            if (unit == null || !unit.activeSelf) continue;

            float distance = Vector2.Distance(origin, unit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestUnit = unit;
            }
        }

        return nearestUnit;
    }

    public GameObject FindFarthestPlayerUnit(Vector3 origin, float radius)
    {
        float farthestDistance = 0f;
        GameObject farthestUnit = null;

        foreach (GameObject unit in _playerUnits)
        {
            if (unit == null || !unit.activeSelf) continue;

            float distance = Vector2.Distance(origin, unit.transform.position);
            if (distance > farthestDistance && distance <= radius)
            {
                farthestDistance = distance;
                farthestUnit = unit;
            }
        }

        return farthestUnit;
    }

    public List<GameObject> GetAllPlayerUnitsInRadius(Vector3 origin, float radius)
    {
        List<GameObject> unitsInRadius = new List<GameObject>();

        foreach (GameObject unit in _playerUnits)
        {
            if (unit == null || !unit.activeSelf) continue;

            float distance = Vector2.Distance(origin, unit.transform.position);
            if (distance <= radius)
            {
                unitsInRadius.Add(unit);
            }
        }

        return unitsInRadius;
    }
}
