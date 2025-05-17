using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MK10Plates : UpgradeAbstract
{
    // Increases Stats
    void Awake()
    {
        GetComponent<Player>().armour += 0.5f;
        GetComponent<Player>().maxHealth += 700f;
        GetComponent<Player>().CurrentHealth += 700f;
    }

    // Remove Stat Changes
    public override void Remove()
    {
        GetComponent<Player>().armour -= 0.5f;
        GetComponent<Player>().maxHealth -= 700f;
        GetComponent<Player>().CurrentHealth -= 700f;
        Destroy(this);
    }
}
