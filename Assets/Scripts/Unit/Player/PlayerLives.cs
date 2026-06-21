using UnityEngine;

public class PlayerLives : Lives
{
    public override void Die()
    {
        PlayerPool.instance.RemoveUnitFromPool(this.gameObject);
        base.Die();
    }

}
