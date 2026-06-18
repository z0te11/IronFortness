using UnityEngine;

public class Lives : MonoBehaviour
{
    [SerializeField] private float _lives;
    private float _defLives;

    public float Live
    {
        set
        { 
            if (value > _defLives) _lives = _defLives;
            else _lives = value;
        }
        get {return _lives;}
    }

    private void Start()
    {
        _defLives = Live;
    }

    public void GetDamage(float damage)
    {
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
}
