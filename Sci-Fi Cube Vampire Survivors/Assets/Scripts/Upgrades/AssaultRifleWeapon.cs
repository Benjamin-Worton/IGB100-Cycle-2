using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifleWeapon : Weapon
{
    private GameObject bulletPrefab;
    private GameObject AssaultRiflePrefab;
    private GameObject AssaultRifle;

    
    [SerializeField] protected float damage = 5f;
    [SerializeField] private float range = 3.5f;
    // Method to attack or fire bullets
    protected override void Attack()
    {
#nullable enable
        GameObject? nearestEnemy = FindNearestEnemy();
        if (nearestEnemy == null) return;
#nullable disable
        Shoot(nearestEnemy);
    }

    private void Awake()
    {
        // Set up weapon prefabs
        AssaultRiflePrefab = Resources.Load<GameObject>("Prefabs/AssaultRifle");
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");

        fireRate = 1f;  // Default fire rate
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
    
    // Method to upgrade fire rate (called by UpgradeMenu)
    public void UpgradeFireRate()
    {
        fireRate *= 1.5f;  // Increase fire rate (faster firing)
    }
#nullable enable
    private GameObject? FindNearestEnemy()
    {
        // Find nearest enemy, if you know of a better way to do this, please do
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (allEnemies.Length == 0) { return null; }

        // Set first enemy as nearest
        GameObject nearestEnemy = allEnemies[0];
        float distanceToNearest = Vector2.Distance(AssaultRifle.transform.position, nearestEnemy.transform.position);

        // Test all other enemies if they are closer
        for (int enemy = 0; enemy < allEnemies.Length; enemy++)
        {
            float distanceToCurrent = Vector2.Distance(AssaultRifle.transform.position, allEnemies[enemy].transform.position);
            if (distanceToCurrent < distanceToNearest)
            {
                nearestEnemy = allEnemies[enemy];
                distanceToNearest = distanceToCurrent;
            }
        }

        if (Vector2.Distance(transform.position, nearestEnemy.transform.position) > range) { return null; }

        return nearestEnemy;
    }
#nullable disable
    private void Shoot(GameObject target)
    {
        // Set direction and rotation for the assault rifle
        Vector3 direction = target.transform.position - AssaultRifle.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        AssaultRifle.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Flip Assault Rifle if necessary
        Vector3 scale = AssaultRifle.transform.localScale;
        scale.y = direction.x < 0 ? -Mathf.Abs(scale.y) : Mathf.Abs(scale.y);
        AssaultRifle.transform.localScale = scale;

        // Instantiate the bullet and it's stats
        GameObject Bullet = Instantiate(bulletPrefab, AssaultRifle.transform.GetChild(0).transform.position, AssaultRifle.transform.rotation);
        Bullet bulletScript = Bullet.GetComponent<Bullet>();
        bulletScript.damage = damage;
        bulletScript.destroyOnCollision = false;
        bulletScript.range = range;
    }
}