using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaSprayer : WeaponAbstract
{
    private GameObject plasmaSprayPrefab;
    private GameObject plasmaSprayObject;
    [SerializeField] public float damage = 50f;


    protected override void Attack()
    {
        Vector3 sprayDirection = GetComponentInParent<Player>().lastKnownDirection.normalized;

        // Calculate 2D angle in degrees
        float angle = Mathf.Atan2(sprayDirection.y, sprayDirection.x) * Mathf.Rad2Deg;

        // Create rotation only around Z-axis
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        plasmaSprayObject = Instantiate(plasmaSprayPrefab, transform.position, rotation);
        Destroy(plasmaSprayObject, 0.2f);
    }

    // Start is called before the first frame update
    void Awake()
    {
        fireRate = 2f; // Default fire rate
        plasmaSprayPrefab = Resources.Load<GameObject>("Prefabs/Plasma Spray");
    }

    public override void Remove()
    {
        Destroy(this);
    }
}
