using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidCollector : UpgradeAbstract
{
    public override void Remove()
    {
        Destroy(this);
    }
}
