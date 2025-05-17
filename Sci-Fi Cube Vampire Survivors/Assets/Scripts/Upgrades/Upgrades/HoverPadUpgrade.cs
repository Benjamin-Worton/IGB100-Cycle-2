using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverPad : MovementUpgradeAbstract
{
    // Increases Stats
    public override float HandleSpeed(float speed)
    {
        speed *= 2f;
        return speed;
    }

    public override void Remove()
    {
        Destroy(this);
    }
}
