using UnityEngine;

public class UnitLives : MonoBehaviour
{
    [SerializeField] private float _lives;
    private float _defLives;

    public float Lives
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
        _defLives = Lives;
    }

    public void GetDamage(float damage)
    {
        Lives -= damage;
        if (Lives <= 0)
        {
            Die();
        }
    }

    public void GetHeal(float heal)
    {
        Lives += heal;
    }

    public virtual void Die()
    {
        Destroy(this.gameObject);
    }
}
