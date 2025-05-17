using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalOverload : UpgradeAbstract
{
    // Increases Stats
    void Awake()
    {
        GetComponent<Player>().critRate += 0.15f;
    }

    // Removes Stat Changes
    public override void Remove()
    {
        GetComponent<Player>().critRate -= 0.15f;
        Destroy(this);
    }
}
