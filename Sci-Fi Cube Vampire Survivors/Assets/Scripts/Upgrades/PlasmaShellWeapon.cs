using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaShell : Weapon
{
    [SerializeField] private float bonusArmour = 0f;
    public float damage = 10f;
    protected override void Attack()
    {
        return;
    }

    // Start is called before the first frame update
    void Awake()
    {
        fireRate = 0;
        this.GetComponent<Player>().armour += 10f;
        bonusArmour += 1f;
    }



    public override void Remove()
    {
        this.GetComponent<Player>().armour -= bonusArmour;
        Destroy(this);
    }

}
