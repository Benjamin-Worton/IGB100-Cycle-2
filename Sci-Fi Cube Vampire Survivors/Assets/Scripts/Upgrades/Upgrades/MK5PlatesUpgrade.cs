using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MK5Plates : UpgradeAbstract
{
    // Increases Stats
    void Awake()
    {
        GetComponent<Player>().armour += 0.3f;
        GetComponent<Player>().maxHealth += 300f;
        GetComponent<Player>().CurrentHealth += 300f;
    }

    // Remove Stat Changes
    public override void Remove()
    {
        GetComponent<Player>().armour -= 0.3f;
        GetComponent<Player>().maxHealth -= 300f;
        GetComponent<Player>().CurrentHealth -= 300f;
        Destroy(this);
    }
}
