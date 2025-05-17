using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MK1Plates : UpgradeAbstract
{
    // Increases Stats
    void Awake()
    {
        GetComponent<Player>().armour += 0.1f;
        GetComponent<Player>().maxHealth += 50;
        GetComponent<Player>().CurrentHealth += 50;

    }

    // Remove Stat Changes
    public override void Remove()
    {
        GetComponent<Player>().armour -= 0.1f;
        GetComponent<Player>().CurrentHealth -= 50;
        Destroy(this);
    }
}
