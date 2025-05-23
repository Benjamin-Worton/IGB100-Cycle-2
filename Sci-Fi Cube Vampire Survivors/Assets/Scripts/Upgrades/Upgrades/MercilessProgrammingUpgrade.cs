using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercilessProgramming : UpgradeAbstract
{
    public override void Remove()
    {
        Destroy(this);
    }
}
