using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
    protected override void Start()
    {
        base.Start();
        PlayerPool.instance.AddUnitToPool(this.gameObject);
    }
}
