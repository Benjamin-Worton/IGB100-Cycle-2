using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornetDrone : MonoBehaviour
{
    private GameObject bulletPrefab;

    [Header("Stats")]
    public float damage = 5f;
    public float range = 3.5f;


    // Start is called before the first frame update
    void Awake()
    {
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
    }

    public void Fire()
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
        if (allEnemies.Length == 0) { return null; }

        // Set first enemy as nearest
        GameObject nearestEnemy = allEnemies[0];
        float distanceToNearest = Vector2.Distance(transform.position, nearestEnemy.transform.position);

        // Test all other enemies if they are closer
        for (int enemy = 0; enemy < allEnemies.Length; enemy++)
        {
            float distanceToCurrent = Vector2.Distance(transform.position, allEnemies[enemy].transform.position);
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
        Vector3 direction = target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Flip Assault Rifle if necessary
        Vector3 scale = transform.localScale;
        scale.x = direction.x < 0 ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;

        // Instantiate the bullet and it's stats
        GameObject Bullet = Instantiate(bulletPrefab, transform.GetChild(0).transform.position, Quaternion.Euler(0f, 0f, angle));
        Bullet bulletScript = Bullet.GetComponent<Bullet>();
        bulletScript.damage = damage;
        bulletScript.destroyOnCollision = true;
        bulletScript.range = range;
    }
}
