using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IonBattery : UpgradeAbstract
{
    // Increases stats
    void Awake()
    {
        GetComponent<Player>().cooldownMultiplier += 0.2f;
        GetComponent<Player>().damageMultiplier += 0.2f;
    }

    // Removes Stats
    public override void Remove()
    {
        GetComponent<Player>().cooldownMultiplier -= 0.2f;
        GetComponent<Player>().damageMultiplier -= 0.2f;
        Destroy(this);
        
    }
}
