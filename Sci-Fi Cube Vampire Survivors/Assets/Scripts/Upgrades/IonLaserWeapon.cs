using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IonLaser : Weapon
{
    private GameObject ionLaserPrefab;
    [SerializeField] private float damage = 50f;
    [SerializeField] private float secondsActive = 0.1f;




    protected override void Attack()
    {
        GameObject IonLaserObject = Instantiate(ionLaserPrefab, transform.position, Quaternion.identity);
        IonLaserObject.GetComponent<IonLaserObject>().damage = damage;
        IonLaserObject.GetComponent<IonLaserObject>().lifetime = secondsActive;
    }

    // Start is called before the first frame update
    void Awake()
    {
        fireRate = 3f; // Default fire rate
        ionLaserPrefab = Resources.Load<GameObject>("Prefabs/IonLaser");
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
