using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuclearRifle : WeaponAbstract, IOrbiting
{
    // Game Objects 
    private GameObject bulletPrefab;
    private GameObject AssaultRiflePrefab;


    [SerializeField] private float damage = 50f;
    [SerializeField] private float range = 4f;

    protected override void Start()
    {
        fireRate = 1f;  // Default fire rate

        // Set up weapon prefabs
        AssaultRiflePrefab = Resources.Load<GameObject>("Prefabs/Nuclear Rifle");
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Rifle Bullet");

        // Add weapon to weapon manager
        weapon = Instantiate(AssaultRiflePrefab, transform.position, Quaternion.identity);
        GetComponent<WeaponManager>().AddWeapon(this);

        base.Start();
    }

    public override void Remove()
    {
        GetComponent<WeaponManager>().RemoveWeapon(this);
        Destroy(weapon);
        Destroy(this);
    }

    void Update()
    {
        if (fireRate == 0f)
        {
            Attack();
        }
    }

    #region Attack Functions 

    // Finds Nearest Enemy and then Shoots at them
    protected override void Attack()
    {
#nullable enable
        GameObject? nearestEnemy = FindNearestEnemy();
        if (nearestEnemy == null) return;
#nullable disable
        Shoot(nearestEnemy);
    }
#nullable enable
    private GameObject? FindNearestEnemy()
    {
        // Find nearest enemy, if you know of a better way to do this, please do
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject? nearestEnemy = null;
        float distanceToNearest = Mathf.Infinity;

        // Test all other enemies if they are closer

        foreach (var enemy in allEnemies)
        {

            bool isValid = false;
            BasicEnemy basicEnemy = enemy.GetComponent<BasicEnemy>();
            if (basicEnemy != null && !basicEnemy.isDead) { isValid = true; }
            Crate crate = enemy.GetComponent<Crate>();
            if (crate != null) { isValid = true; }

            if (!isValid) { continue; }

            float distanceToCurrent = Vector2.Distance(weapon.transform.position, enemy.transform.position);
            if (distanceToCurrent < distanceToNearest) { distanceToNearest = distanceToCurrent; nearestEnemy = enemy; }

        }

        if (nearestEnemy != null && Vector2.Distance(weapon.transform.position, nearestEnemy.transform.position) < range)
        {
            if (fireRate == 0f)
            {
                fireRate = 1f;
                StartCoroutine(FireLoop());
            }

            return nearestEnemy;
        }

        return null;
    }
#nullable disable

    private void Shoot(GameObject target)
    {
        // Set direction and rotation for the assault rifle
        Vector3 direction = target.transform.position - weapon.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        weapon.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Flip Assault Rifle if necessary
        Vector3 scale = weapon.transform.localScale;
        scale.y = direction.x < 0 ? -Mathf.Abs(scale.y) : Mathf.Abs(scale.y);
        weapon.transform.localScale = scale;

        // Instantiate the bullet and it's stats
        GameObject Bullet = Instantiate(bulletPrefab, weapon.transform.GetChild(0).transform.position, weapon.transform.rotation);
        Bullet bulletScript = Bullet.GetComponent<Bullet>();
        bulletScript.damage = damage;
        bulletScript.destroyOnCollision = true;
    }
    #endregion Attack 
}