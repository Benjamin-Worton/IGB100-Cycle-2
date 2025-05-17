using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fabricator : UpgradeAbstract
{
    // Increases Stats
    void Awake()
    {
        GetComponent<Player>().regen += 0.2f;
    }

    // Removes Stat Changes
    public override void Remove()
    {
        GetComponent<Player>().regen -= 0.2f;
        Destroy(this);
    }
}
