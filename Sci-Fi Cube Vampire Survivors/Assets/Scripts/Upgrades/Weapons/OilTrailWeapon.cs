using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilTrail : WeaponAbstract
{
    private GameObject oilPrefab;
    private readonly float burnDistance = 1f;
    [SerializeField] private float burnTime = 5f;


    protected override void Attack()
    {
        // Create Oil Trail and set it's stats
        GameObject Oil = Instantiate(oilPrefab, transform.position + Vector3.forward * 0.01f, Quaternion.identity);
        Oil.GetComponent<Oil>().burnTime = burnTime;
        Oil.GetComponent<Oil>().burnDistance = burnDistance;

    }

    void Awake()
    {
        fireRate = 0.2f; // Default fire rate
        oilPrefab = Resources.Load<GameObject>("Prefabs/Oil");
    }

    public override void Remove()
    {
        Destroy(this);
    }
}
