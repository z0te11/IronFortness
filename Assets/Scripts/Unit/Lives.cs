using UnityEngine;

public class Lives : MonoBehaviour, ILives
{
    [SerializeField] private float _lives;
    private float _defLives;
    [SerializeField] private DamageFeedback _damageFeedback;

    public float Live
    {
        set
        { 
            _lives = value;
        }
        get {return _lives;}
    }

    protected virtual void Start()
    {
        _defLives = Live;
    }

    public void GetDamage(float damage)
    {

        if (_damageFeedback != null)
        {
            _damageFeedback.PlayDamageFlash();
        }

        Live -= damage;
        if (Live <= 0)
        {
            Die();
        }
    }

    public void GetHeal(float heal)
    {
        Live += heal;
    }

    public virtual void Die()
    {
        Destroy(this.gameObject);
    }

    public void SetLives(int newLives)
    {
        Live = newLives;
    }
}
