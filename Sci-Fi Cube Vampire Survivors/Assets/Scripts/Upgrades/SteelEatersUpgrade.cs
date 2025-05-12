using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelEaters : Weapon
{
    [SerializeField] private float LifeSteal = 0.1f;
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
    public void GiveHealth(float damage)
    {
        this.GetComponent<Player>().TakeDamage(-damage * LifeSteal);
    }

    public override void Remove()
    {
        Destroy(this);
    }
}
