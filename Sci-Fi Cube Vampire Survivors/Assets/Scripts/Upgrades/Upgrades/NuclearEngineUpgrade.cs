using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuclearEngine : MovementUpgradeAbstract
{
    // Increases Stats
    public override float HandleSpeed(float speed)
    {
        speed *= 1.3f;
        return speed;
    }

    public override void Remove()
    {
        Destroy(this);
    }
}
