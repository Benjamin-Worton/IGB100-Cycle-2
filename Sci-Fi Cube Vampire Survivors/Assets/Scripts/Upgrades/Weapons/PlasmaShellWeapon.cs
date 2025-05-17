using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaShell : WeaponAbstract
{
    [SerializeField] private float Armour = 0f;
    public float damage = 10f;
    protected override void Attack()
    {
        return;
    }

    void Awake()
    {
        fireRate = 0;
        Armour += 0.05f;
        GetComponent<Player>().armour += Armour;
        
    }

    public override void Remove()
    {
        GetComponent<Player>().armour -= Armour;
        Destroy(this);
    }
}
