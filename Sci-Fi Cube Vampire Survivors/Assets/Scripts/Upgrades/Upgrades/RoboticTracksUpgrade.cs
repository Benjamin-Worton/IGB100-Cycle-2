using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoboticTracks : MovementUpgradeAbstract
{
    // Increases Stats when moving in cardinal directions
    public override float HandleSpeed(float speed)
    {
        Vector3 direction = gameObject.GetComponent<Player>().direction;

        if (direction.x == 0 || direction.y == 0)
        {
            speed *= 3f;
        } else
        {
            speed *= 0.5f;
        }
        return speed;
    }

    public override void Remove()
    { 
        Destroy(this);
    }
}
