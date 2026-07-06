// SlowBullet.cs
using UnityEngine;

public class SlowBullet : Bullet
{
    [Header("Настройки замедления")]
    [SerializeField] private float _slowAmount = 0.5f; // Множитель скорости (0.5 = замедление на 50%)
    [SerializeField] private float _slowDuration = 2f; // Длительность замедления в секундах
    
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        EnemyLives unitLives = other.GetComponent<EnemyLives>();
        
        if (unitLives != null)
        {
            // Наносим урон
            unitLives.GetDamage(_damage);
            
            // Применяем замедление
            UnitMoveToBehavior enemyMovement = unitLives.GetComponent<UnitMoveToBehavior>();
            if (enemyMovement != null)
            {
                enemyMovement.ApplySlow(_slowAmount, _slowDuration);
            }
            
            Destroy(gameObject);
        }
    }
}
