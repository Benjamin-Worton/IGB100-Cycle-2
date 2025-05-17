using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelEaters : UpgradeAbstract
{
    // Increases Stats
    void Awake()
    {
        GetComponent<Player>().damageMultiplier += 0.2f;
    }

    // Heal 10% of the damage dealt, called by enemy OnTakeDamageEffects()
    public void GiveHealth(float damage)
    {
        GetComponent<Player>().TakeDamage(-damage * 0.1f);
    }

    // Removes Stat Changes
    public override void Remove()
    {
        GetComponent<Player>().damageMultiplier -= 0.2f;
        Destroy(this);
    }
}
