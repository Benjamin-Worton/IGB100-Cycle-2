using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoulderLasers : Weapon
{
    private GameObject laserPrefab;
    private float DistanceFromPlayer = 0.2f;

    protected override void Attack()
    {
        Instantiate(laserPrefab, transform.position + Vector3.right * DistanceFromPlayer, Quaternion.identity);
        GameObject flippedLaser = Instantiate(laserPrefab, transform.position + Vector3.right * -DistanceFromPlayer, Quaternion.identity);
        flippedLaser.transform.Rotate(0, 180f, 0);
    }

    // Start is called before the first frame update
    void Awake()
    {
        laserPrefab = Resources.Load<GameObject>("Prefabs/Laser");
    }

    public override void Remove()
    {
        Destroy(this);
    }
}
