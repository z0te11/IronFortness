// EnemyMoverToBaseBehavior.cs
using UnityEngine;

public class EnemyMoverToBaseBehavior : UnitMoveToBehavior
{

    public override float Behavior()
    {
        GameObject playerUnit = PlayerPool.instance.FindNearestPlayerUnit(transform.position, _searchRadius);
        
        if (playerUnit == null)
        {
            return 0.5f;
        }
        
        return 0f;
    }
    
    public override void Realize()
    {
        GameObject mainBuilding = PlayerPool.instance.GetMainBuildPlayer();
        
        if (mainBuilding == null) return;
        
        Vector2 direction = (mainBuilding.transform.position - transform.position).normalized;
        _rb.velocity = direction * _speed;
        FaceDirection(direction.x);
    }
}
