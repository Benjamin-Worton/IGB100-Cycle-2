using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaShell : WeaponAbstract
{
    public float damage = 10f;
    protected override void Attack()
    {
        return;
    }

    void Awake()
    {
        fireRate = 0;
        GetComponent<Player>().armour += 0.05f;
        
    }

    public override void Remove()
    {
        GetComponent<Player>().armour -= 0.05f;
        Destroy(this);
    }
}
