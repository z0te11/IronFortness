

using System;
using UnityEngine;

public class EnemyLives : Lives, IGetMoney
{
    [SerializeField] private int _money;
    public override void Die()
    {
        EnemyPool.instance.RemoveUnitFromPool(this.gameObject);
        GetMoney();
        base.Die();
    }

    public void GetMoney()
    {
        MoneyManager.instance.AddMoney(_money);
    }
}
