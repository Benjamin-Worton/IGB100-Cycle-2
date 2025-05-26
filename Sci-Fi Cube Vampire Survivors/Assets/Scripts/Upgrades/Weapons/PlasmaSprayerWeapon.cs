using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaSprayer : WeaponAbstract
{
    private GameObject plasmaSprayPrefab;
    private GameObject plasmaSprayObject;
    private readonly float DistanceFromPlayer = 1.5f;
    [SerializeField] public float damage = 50f;


    protected override void Attack()
    {
        Vector3 sprayDirection = GetComponentInParent<Player>().direction.normalized;

        // Calculate 2D angle in degrees
        float angle = Mathf.Atan2(sprayDirection.y, sprayDirection.x) * Mathf.Rad2Deg;

        // Create rotation only around Z-axis
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        plasmaSprayObject = Instantiate(plasmaSprayPrefab, transform.position + sprayDirection * DistanceFromPlayer, rotation);
        StartCoroutine(RemoveSpray());
    }

    private IEnumerator RemoveSpray()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(plasmaSprayObject);
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
