using UnityEngine;

public class PlayerLives : Lives, ILives
{
    public override void Die()
    {
        PlayerPool.instance.RemoveUnitFromPool(this.gameObject);
        base.Die();
    }

    public void SetLives(int newLives)
    {
        Live = newLives;
    }
}
