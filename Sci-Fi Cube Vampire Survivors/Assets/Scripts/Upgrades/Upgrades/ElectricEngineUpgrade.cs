using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricEngine : MovementUpgradeAbstract
{
    // Increases Stats
    public override float HandleSpeed(float speed)
    {
        speed *= 1.1f;
        return speed;
    }

    public override void Remove()
    {
        Destroy(this);
    }
}
