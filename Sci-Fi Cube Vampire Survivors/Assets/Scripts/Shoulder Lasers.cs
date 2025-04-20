using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoulderLasers : Weapon
{
    private GameObject laserPrefab;
    private float DistanceFromPlayer = 0.2f;

    protected override void Attack()
    {
        GameObject RightLaser = Instantiate(laserPrefab, transform.position + Vector3.right * DistanceFromPlayer, Quaternion.identity);
        GameObject LeftLaser = Instantiate(laserPrefab, transform.position + Vector3.right * -DistanceFromPlayer, Quaternion.identity);
        LeftLaser.transform.Rotate(0, 180f, 0);
        RightLaser.GetComponent<Bullet>().damage = 5f;
        LeftLaser.GetComponent<Bullet>().damage = 5f;
        RightLaser.GetComponent<Bullet>().destroyOnCollision = true;
        LeftLaser.GetComponent<Bullet>().destroyOnCollision = true;
    }

    // Start is called before the first frame update
    void Awake()
    {
        laserPrefab = Resources.Load<GameObject>("Prefabs/Laser");
        fireRate = 3f;
    }

    public override void Remove()
    {
        Destroy(this);
    }
}
