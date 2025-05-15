using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiMatterBattery : Weapon
{
    [SerializeField] private float cooldownMultiplier = 1f;
    [SerializeField] private float damageMultiplier = 1f;

    protected override void Attack()
    {
        return;
    }

    // Start is called before the first frame update
    void Awake()
    {
        fireRate = 0;
        cooldownMultiplier -= 0.2f;
        damageMultiplier -= 0.2f;
        GetComponent<Player>().cooldownMultiplier += cooldownMultiplier;
        GetComponent<Player>().damageMultiplier += damageMultiplier;
    }

    public override void Remove()
    {
        Destroy(this);
        GetComponent<Player>().cooldownMultiplier -= cooldownMultiplier;
        GetComponent<Player>().damageMultiplier -= damageMultiplier;
    }
}
