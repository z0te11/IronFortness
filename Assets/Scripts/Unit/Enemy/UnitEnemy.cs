using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitEnemy : Unit
{
    protected override void Start()
    {
        base.Start();
        EnemyPool.instance.SetUnitInPool(this.gameObject);
    }
}
