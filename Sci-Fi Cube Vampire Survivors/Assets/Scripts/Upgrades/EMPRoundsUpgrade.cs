using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPRounds : Weapon
{
    public float StunDuration = 1f;

    protected override void Attack()
    {
        return;
    }

    // Start is called before the first frame update
    void Awake()
    {
        fireRate = 0;
    }

    public override void Remove()
    {
        Destroy(this);
    }
}
