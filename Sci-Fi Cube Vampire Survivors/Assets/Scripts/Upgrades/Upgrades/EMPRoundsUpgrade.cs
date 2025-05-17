using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPRounds : UpgradeAbstract
{
    // Variable for the enemies to reference
    public float StunDuration = 1f;

    public override void Remove()
    {
        Destroy(this);
    }
}
