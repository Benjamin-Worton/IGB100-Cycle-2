using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveRounds : UpgradeAbstract
{
    public override void Remove()
    {
        Destroy(this);
    }
}
