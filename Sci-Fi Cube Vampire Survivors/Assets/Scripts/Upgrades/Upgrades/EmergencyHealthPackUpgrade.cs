using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyHealthPack : UpgradeAbstract
{
    // Called when enemies die
    public void DropHealthPack()
    {
        if (gameObject.GetComponent<Player>().CurrentHealth / gameObject.GetComponent<Player>().maxHealth > 0.5) { return; }
        if (Random.Range(1,100) <= 5) { return; }
        Instantiate(Resources.Load<GameObject>("Prefabs/Health"));
    }

    // Remove stats
    public override void Remove()
    {
        Destroy(this);
    }
}
