using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalAmmunition : UpgradeAbstract
{
    // Increases Stats
    void Awake()
    {
        GetComponent<Player>().critRate += 0.05f;
    }

    // Removes Stat Changes
    public override void Remove()
    {
        GetComponent<Player>().critRate -= 0.05f;
        Destroy(this);
    }
}
