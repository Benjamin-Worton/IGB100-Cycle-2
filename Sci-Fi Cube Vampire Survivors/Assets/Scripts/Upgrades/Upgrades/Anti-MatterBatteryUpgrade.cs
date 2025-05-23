using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiMatterBattery : UpgradeAbstract
{
    // Reduces
    void Awake()
    {
        GetComponent<Player>().CooldownMultiplier -= 0.2f;
        GetComponent<Player>().damageMultiplier -= 0.05f;
    }

    // Removes Stat Changes
    public override void Remove()
    {
        GetComponent<Player>().CooldownMultiplier += 0.2f;
        GetComponent<Player>().damageMultiplier += 0.05f;
        Destroy(this);
        
    }
}
