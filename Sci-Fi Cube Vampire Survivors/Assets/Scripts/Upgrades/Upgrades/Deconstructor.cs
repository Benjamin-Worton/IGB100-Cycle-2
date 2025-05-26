using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deconstructor : UpgradeAbstract
{
    public override void Remove()
    {
        Destroy(this);
    }
}
