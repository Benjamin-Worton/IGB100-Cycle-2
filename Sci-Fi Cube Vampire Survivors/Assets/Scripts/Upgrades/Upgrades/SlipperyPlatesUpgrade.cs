using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipperyPlates : UpgradeAbstract
{
    // Increases Stats
    void Awake()
    {
        GetComponent<Player>().critBlock += 0.05f;
    }

    // Removes Stat Changes
    public override void Remove()
    {
        GetComponent<Player>().critBlock -= 0.05f;
        Destroy(this);
    }
}
