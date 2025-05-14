using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoulderLasers : Weapon
{
    private GameObject laserPrefab;
    private readonly float DistanceFromPlayer = 0.2f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 7f;


    protected override void Attack()
    {
        GameObject RightLaser = Instantiate(laserPrefab, transform.position + Vector3.right * DistanceFromPlayer, Quaternion.identity);
        GameObject LeftLaser = Instantiate(laserPrefab, transform.position + Vector3.right * -DistanceFromPlayer, Quaternion.identity);
        LeftLaser.transform.Rotate(0, 180f, 0);
        RightLaser.GetComponent<Bullet>().damage = damage;
        LeftLaser.GetComponent<Bullet>().damage = damage;
        RightLaser.GetComponent<Bullet>().destroyOnCollision = true;
        LeftLaser.GetComponent<Bullet>().destroyOnCollision = true;
        RightLaser.GetComponent<Bullet>().range = range;
        LeftLaser.GetComponent<Bullet>().range = range;
    }

    // Start is called before the first frame update
    void Awake()
    {
        fireRate = 1f; // Default fire rate
        laserPrefab = Resources.Load<GameObject>("Prefabs/Laser");
    }

    public override void Remove()
    {
        Destroy(this);
    }

    // Method to upgrade fire rate (called by UpgradeMenu)
    public void UpgradeFireRate()
    {
        fireRate /= 1.5f;  // Increase fire rate (faster firing)
    }
}
