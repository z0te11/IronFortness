// EnemyMoveToPlayerBehavior.cs
using UnityEngine;

public class EnemyMoveToPlayerBehavior : MonoBehaviour, IBehavior
{
    [Header("Настройки движения")]
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _searchRadius = 5f;
    
    private Rigidbody2D _rb;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            _rb = gameObject.AddComponent<Rigidbody2D>();
            _rb.gravityScale = 0;
        }
    }
    
    public float Behavior()
    {
        GameObject playerUnit = PlayerPool.instance.FindNearestPlayerUnit(transform.position, _searchRadius);
        
        if (playerUnit != null)
        {
            return 0.8f;
        }
        
        return 0f;
    }
    
    public void Realize()
    {
        GameObject playerUnit = PlayerPool.instance.FindNearestPlayerUnit(transform.position, _searchRadius);
        
        if (playerUnit == null) return;
        
        Vector2 direction = (playerUnit.transform.position - transform.position).normalized;
        _rb.velocity = direction * _speed;
    }
}
