using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctagonalLasers : WeaponAbstract
{
    private GameObject laserPrefab;
    private readonly float DistanceFromPlayer = 0.4f;
    [SerializeField] private float damage = 50f;


    protected override void Attack()
    {
        for (int i = 0; i < 8; i++)
        {
            float angle = (360f / 8) * i;
            Vector3 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
            GameObject Laser = Instantiate(laserPrefab, transform.position + direction * DistanceFromPlayer, Quaternion.identity);
            Laser.transform.Rotate(0, 0, angle);
            Laser.GetComponent<Bullet>().damage = damage;
            Laser.GetComponent<Bullet>().isBurning = true;

        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        fireRate = 1f; // Default fire rate
        laserPrefab = Resources.Load<GameObject>("Prefabs/Laser");
        StartCoroutine(DelayBetweenWeapons());
    }

    private IEnumerator DelayBetweenWeapons()
    {
        foreach (var script in gameObject.GetComponents<EMP1Mines>())
        {
            yield return new WaitForSeconds(fireRate / 5);
        }
        base.Start();
    }

    public override void Remove()
    {
        Destroy(this);
    }
}
