using UnityEngine;

public class UnitPlayerLives : UnitLives
{
    public override void Die()
    {
        PlayerPool.instance.RemoveUnitFromPool(this.gameObject);
        base.Die();
    }
}
