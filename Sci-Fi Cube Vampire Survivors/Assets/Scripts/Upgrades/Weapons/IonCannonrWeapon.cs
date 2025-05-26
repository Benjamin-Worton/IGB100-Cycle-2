using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IonCannon : WeaponAbstract
{
    private GameObject ionLaserPrefab;
    [SerializeField] private float damage = 50f;
    [SerializeField] private float secondsActive = 0.1f;

    protected override void Attack()
    {
        // Create Ion Laser Object and set it's stats
        GameObject IonLaserObject = Instantiate(ionLaserPrefab, transform.position, Quaternion.identity);
        IonLaserObject.GetComponent<IonLaserObject>().damage = damage;
        IonLaserObject.GetComponent<IonLaserObject>().lifetime = secondsActive;
    }

    protected override void Start()
    {
        fireRate = 3f; // Default fire rate
        ionLaserPrefab = Resources.Load<GameObject>("Prefabs/IonLaser");
        StartCoroutine(DelayBetweenWeapons());
    }

    private IEnumerator DelayBetweenWeapons()
    {
        foreach (var script in gameObject.GetComponents<IonCannon>())
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
