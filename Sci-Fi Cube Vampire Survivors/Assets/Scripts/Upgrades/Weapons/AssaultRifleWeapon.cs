using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : WeaponAbstract, IOrbiting
{
    // Game Objects 
    private GameObject bulletPrefab;
    private GameObject AssaultRiflePrefab;

    
    [SerializeField] private float damage = 5f;
    [SerializeField] private float range = 4f;

    protected override void Start()
    {
        fireRate = 1f;  // Default fire rate

        // Set up weapon prefabs
        AssaultRiflePrefab = Resources.Load<GameObject>("Prefabs/Assault Rifle");
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");

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
        if (allEnemies.Length == 0) { fireRate = 0f;  return null; }

        // Set first enemy as nearest
        GameObject nearestEnemy = allEnemies[0];
        float distanceToNearest = Vector2.Distance(weapon.transform.position, nearestEnemy.transform.position);

        // Test all other enemies if they are closer
        for (int enemy = 0; enemy < allEnemies.Length; enemy++)
        {
            float distanceToCurrent = Vector2.Distance(weapon.transform.position, allEnemies[enemy].transform.position);
            if (allEnemies[enemy].GetComponent<BasicEnemy>() != null && !allEnemies[enemy].GetComponent<BasicEnemy>().isDead) { continue; }
            if (distanceToCurrent < distanceToNearest)
            {
                nearestEnemy = allEnemies[enemy];
                distanceToNearest = distanceToCurrent;
            }
        }

        // If nearest enemy is out of range, return null and try again next frame
        if (Vector2.Distance(transform.position, nearestEnemy.transform.position) > range) { fireRate = 0f; return null; }

        if (fireRate == 0f)
        {
            fireRate = 1f;
            StartCoroutine(FireLoop());
        }
        return nearestEnemy;
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
        bulletScript.range = range;
    }
#endregion Attack 
}