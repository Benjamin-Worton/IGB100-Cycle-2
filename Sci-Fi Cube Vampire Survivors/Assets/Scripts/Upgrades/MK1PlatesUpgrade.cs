using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MK1Plates : Weapon
{
    [SerializeField] private float Armour = 0f;
    protected override void Attack()
    {
        return;
    }

    // Start is called before the first frame update
    void Awake()
    {
        fireRate = 0;
        Armour = 0.1f;
        GetComponent<Player>().armour += Armour;
        GetComponent<Player>().maxHealth += 50;

    }

    public override void Remove()
    {
        GetComponent<Player>().armour -= Armour;
        GetComponent<Player>().maxHealth -= 50;
        Destroy(this);
    }

    public void UpgradeArmour()
    {
        GetComponent<Player>().armour -= Armour;
        Armour += (1f - Armour) * 0.1f;  // Increase armour
        GetComponent<Player>().armour += Armour;
    }
}
