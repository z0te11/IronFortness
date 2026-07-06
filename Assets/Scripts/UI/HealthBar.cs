using UnityEngine.UI;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        float healthPercent = currentHealth / maxHealth;
        _healthBar.fillAmount = healthPercent;
    }
}
