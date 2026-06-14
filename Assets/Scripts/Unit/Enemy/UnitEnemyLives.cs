

public class UnitEnemyLives : UnitLives
{
    public override void Die()
    {
        EnemyPool.instance.RemoveUnitFromPool(this.gameObject);
        base.Die();
    }
}
