using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fabricator : Weapon
{
    [SerializeField] private float regenIncrease = 0f;

    protected override void Attack()
    {
        return;
    }

    // Start is called before the first frame update
    void Awake()
    {
        fireRate = 0;
        regenIncrease += 0.2f;
        GetComponent<Player>().regen += regenIncrease;
    }

    public override void Remove()
    {
        Destroy(this);
        GetComponent<Player>().regen -= regenIncrease;
    }
}
