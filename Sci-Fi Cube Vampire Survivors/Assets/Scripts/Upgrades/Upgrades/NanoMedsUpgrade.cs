using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NanoMeds : UpgradeAbstract
{
    // Increases Stats
    void Awake()
    {
        GetComponent<Player>().regen += 1f;
    }

    // Removes Stat Changes
    public override void Remove()
    {
        GetComponent<Player>().regen -= 1f;
        Destroy(this);
    }
}
