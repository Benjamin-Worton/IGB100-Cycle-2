using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoboticTracks : Weapon
{
    [SerializeField] private float SpeedIncrease = 2f;
    [SerializeField] private float SpeedDecrease = 0.5f;

    protected override void Attack()
    {
        return;
    }

    // Start is called before the first frame update
    void Awake()
    {
        fireRate = 0;
    }

    // Update is called once per frame
    public float HandleSpeed(float speed)
    {
        Vector3 direction = gameObject.GetComponent<Player>().direction;

        if (direction.x == 0 || direction.y == 0)
        {
            speed *= 1f + SpeedIncrease;
        } else
        {
            speed *= 1f - SpeedDecrease;
        }
        return speed;
    }

    public override void Remove()
    { 
        Destroy(this);
    }
}
