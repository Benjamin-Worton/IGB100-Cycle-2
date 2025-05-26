using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyHealthDrones : UpgradeAbstract
{
    // Called when enemies die
    public void DropHealthPack()
    {
        if (Random.Range(1,100) <= 15) { return; }
        Instantiate(Resources.Load<GameObject>("Prefabs/Health"));
    }

    // Remove stats
    public override void Remove()
    {
        Destroy(this);
    }
}
