using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class AssaultRifleWeapon : Weapon
{
    private GameObject bulletPrefab;
    private GameObject AssaultRiflePrefab;
    private GameObject AssaultRifle;

    protected override void Attack()
    {
        // Find nearest enemy, if you know of a better way to do this, please do
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (allEnemies.Length == 0) { return; }
        GameObject nearestEnemy = allEnemies[0];
        float distanceToNearest = Vector2.Distance(this.transform.position, nearestEnemy.transform.position);

        for (int enemy = 0; enemy < allEnemies.Length; enemy++)
        {
            float distanceToCurrent = Vector2.Distance(this.transform.position, allEnemies[enemy].transform.position);
            if (distanceToCurrent < distanceToNearest)
            {
                nearestEnemy = allEnemies[enemy];
                distanceToNearest = distanceToCurrent;
            }
        }

        Vector3 direction = nearestEnemy.transform.position - AssaultRifle.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Vector3 scale = AssaultRifle.transform.localScale;
        scale.y = direction.x < 0 ? -Mathf.Abs(scale.y) : Mathf.Abs(scale.y);
        AssaultRifle.transform.localScale = scale;

        AssaultRifle.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Instantiate(bulletPrefab, AssaultRifle.transform.GetChild(0).transform.position, AssaultRifle.transform.rotation);
    }

    private void Awake()
    {
        // Set up weapon prefabs
        AssaultRiflePrefab = Resources.Load<GameObject>("Prefabs/AssaultRifle");
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");

        fireRate = 1f;
        AssaultRifle = Instantiate(AssaultRiflePrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
    }

    void Update()
    {
        // Keep assault rifle locked to player
        AssaultRifle.transform.position = this.transform.position + Vector3.up * 0.5f;
    }
    public override void Remove()
    {
        Destroy(AssaultRifle);
        Destroy(this);
    }
}
