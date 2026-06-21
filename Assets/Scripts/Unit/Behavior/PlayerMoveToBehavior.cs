// EnemyMoveToPlayerBehavior.cs
using UnityEngine;

public class PlayerMoveToBehavior : UnitMoveToBehavior
{
    [SerializeField] protected UnitMovement _unitMovement;
    
    public override void Realize()
    {   
        if (_unitMovement != null)
        {
           if (_unitMovement.isMoving) return;
        }
        if (_targetUnit == null) return;
        
        Vector2 direction = (_targetUnit.transform.position - transform.position).normalized;
        _rb.velocity = direction * _speed;
        FaceDirection(direction.x);
    }
}
