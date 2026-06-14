using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    private List<GameObject> _enemyPoolUnits = new List<GameObject>();

    public static EnemyPool instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void SetUnitInPool(GameObject newUnit)
    {
        _enemyPoolUnits.Add(newUnit);
    }

    public void RemoveUnitFromPool(GameObject newUnit)
    {
        _enemyPoolUnits.Remove(newUnit);
    }

    public GameObject FindNearestTarget(Vector3 origin, float radius)
    {
        float closestDistance = radius;
        GameObject nearestTarget = null;
        
        foreach (GameObject target in _enemyPoolUnits)
        {
            if (target == null || !target.activeSelf) continue;
            
            float distance = Vector2.Distance(origin, target.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestTarget = target;
            }
        }
        
        return nearestTarget;
    }

    public GameObject FindFarthestTarget(Vector3 origin, float radius)
    {
        float farthestDistance = 0f;
        GameObject farthestTarget = null;
        
        foreach (GameObject target in _enemyPoolUnits)
        {
            if (target == null || !target.activeSelf) continue;
            
            float distance = Vector2.Distance(origin, target.transform.position);
            if (distance > farthestDistance && distance <= radius)
            {
                farthestDistance = distance;
                farthestTarget = target;
            }
        }
        
        return farthestTarget;
    }
}