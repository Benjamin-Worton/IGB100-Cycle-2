using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhasePlates : UpgradeAbstract
{
    // Increases Stats
    void Awake()
    {
        GetComponent<Player>().critBlock += 0.15f;
    }

    // Removes Stat Changes
    public override void Remove()
    {
        GetComponent<Player>().critBlock -= 0.15f;
        Destroy(this);
    }
}
