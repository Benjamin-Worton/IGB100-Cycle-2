using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelEaters : Weapon
{
    [SerializeField] private float LifeSteal = 0.1f;
    [SerializeField] private float BonusDamage = 0.2f;

    protected override void Attack()
    {
        return;
    }

    // Start is called before the first frame update
    void Awake()
    {
        fireRate = 0;
        GetComponent<Player>().damageMultiplier += BonusDamage;
    }

    // Update is called once per frame
    public void GiveHealth(float damage)
    {
        GetComponent<Player>().TakeDamage(-damage * LifeSteal);
    }

    public override void Remove()
    {
        GetComponent<Player>().damageMultiplier -= BonusDamage;
        Destroy(this);
    }
}
