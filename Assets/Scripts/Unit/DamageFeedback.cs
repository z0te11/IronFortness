// DamageFeedback.cs
using UnityEngine;
using System.Collections;

public class DamageFeedback : MonoBehaviour
{
    [Header("Настройки мерцания")]
    [SerializeField] private float _flashDuration = 0.2f;
    [SerializeField] private Color _flashColor = Color.red;
    
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    private Coroutine _flashCoroutine;

    private void Start()
    {

        if (_spriteRenderer != null)
        {
            _originalColor = _spriteRenderer.color;
        }
    }

    public void PlayDamageFlash()
    {
        if (_spriteRenderer == null) return;

        // Если уже идёт мерцание, останови его
        if (_flashCoroutine != null)
        {
            StopCoroutine(_flashCoroutine);
        }

        _flashCoroutine = StartCoroutine(FlashRed());
    }

    private IEnumerator FlashRed()
    {
        // Меняем цвет на красный
        _spriteRenderer.color = _flashColor;
        
        // Ждём
        yield return new WaitForSeconds(_flashDuration);
        
        // Возвращаем оригинальный цвет
        _spriteRenderer.color = _originalColor;
        
        _flashCoroutine = null;
    }
}
