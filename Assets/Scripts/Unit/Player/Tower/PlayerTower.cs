using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTower : Unit
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        PlayerPool.instance.AddUnitToPool(this.gameObject);
    }

}
