using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IonBattery : UpgradeAbstract
{
    // Increases stats
    void Awake()
    {
        GetComponent<Player>().CooldownMultiplier += 0.05f;
        GetComponent<Player>().damageMultiplier += 0.2f;
    }

    // Removes Stats
    public override void Remove()
    {
        GetComponent<Player>().CooldownMultiplier -= 0.05f;
        GetComponent<Player>().damageMultiplier -= 0.2f;
        Destroy(this);
        
    }
}
