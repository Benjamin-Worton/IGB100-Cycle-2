using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoulderLasers : WeaponAbstract
{
    private GameObject laserPrefab;
    private readonly float DistanceFromPlayer = 0.4f;
    [SerializeField] private float damage = 10f;


    protected override void Attack()
    {
        GameObject RightLaser = Instantiate(laserPrefab, transform.position + Vector3.right * DistanceFromPlayer, Quaternion.identity);
        GameObject LeftLaser = Instantiate(laserPrefab, transform.position + Vector3.right * -DistanceFromPlayer, Quaternion.identity);
        LeftLaser.transform.Rotate(0, 180f, 0);
        RightLaser.GetComponent<Bullet>().damage = damage;
        LeftLaser.GetComponent<Bullet>().damage = damage;
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

    // Method to upgrade fire rate (called by UpgradeMenu) ### MUST REMOVE ###
    public void UpgradeFireRate()
    {
        fireRate /= 1.5f;  // Increase fire rate (faster firing)
    }
}
