using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverPad : Weapon
{
    [SerializeField] private float SpeedIncrease = 1f;
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
        speed *= 1f + SpeedIncrease;
        return speed;
    }

    public override void Remove()
    {
        Destroy(this);
    }
}
