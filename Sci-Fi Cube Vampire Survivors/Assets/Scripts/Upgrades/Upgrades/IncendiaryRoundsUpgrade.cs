using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncendiaryRounds : UpgradeAbstract
{
    public override void Remove()
    {
        Destroy(this);
    }
}
