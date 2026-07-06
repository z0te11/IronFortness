// Cannonball.cs
using UnityEngine;

public class Cannonball : Bullet
{
    [Header("Настройки пушечного ядра")]
    [SerializeField] private float _explosionRadius = 3f;
    [SerializeField] private LayerMask _targetLayers = -1; // По умолчанию все слои
    [SerializeField] private GameObject _explosionEffect; // Опциональный эффект взрыва
    [SerializeField] private bool _destroyOnImpact = true; // Уничтожать ли ядро при попадании
    
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        Explode();
    }
    
    private void Explode()
    {
        // Создаем эффект взрыва, если он задан
        if (_explosionEffect != null)
        {
            Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        }
        
        // Находим все коллайдеры в радиусе взрыва
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(
            transform.position, 
            _explosionRadius, 
            _targetLayers
        );
        
        // Наносим урон всем врагам в зоне поражения
        foreach (Collider2D hitCollider in hitColliders)
        {
            EnemyLives unitLives = hitCollider.GetComponent<EnemyLives>();
            if (unitLives != null)
            {
                // Можно добавить урон по убыванию от центра взрыва
                float distance = Vector2.Distance(transform.position, hitCollider.transform.position);
                float damageMultiplier = 1f - (distance / _explosionRadius); // Линейное падение урона
                float finalDamage = _damage * Mathf.Clamp01(damageMultiplier);
                
                unitLives.GetDamage(finalDamage);
            }
        }
        
        // Уничтожаем ядро после взрыва, если нужно
        if (_destroyOnImpact)
        {
            Destroy(gameObject);
        }
    }
    
    // Для отладки - визуализация радиуса взрыва в редакторе
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.5f); // Оранжевый полупрозрачный
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}