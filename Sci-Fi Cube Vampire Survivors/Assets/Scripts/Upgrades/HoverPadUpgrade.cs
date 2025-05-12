using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverPad : Weapon
{
    [SerializeField] private float SpeedMultiplier = 1.2f;
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
    public float HandleSpeed(float speed)
    {
        Vector3 direction = gameObject.GetComponent<Player>().direction;

        if (direction.x == 0 || direction.y == 0)
        {
            speed = speed * SpeedMultiplier;
        }
        return speed;
    }

    public override void Remove()
    {
        Destroy(this);
    }
}
