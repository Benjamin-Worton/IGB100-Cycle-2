using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP1Mines : WeaponAbstract
{
    private GameObject minePrefab;
    [SerializeField] private float damage = 50f;

    protected override void Attack()
    {
        GameObject Mine = Instantiate(minePrefab, transform.position + Vector3.forward * 0.1f, Quaternion.identity); // Creates Mine and sets damage
        Mine.GetComponent<Mine>().damage = damage;

    }

    // Start is called before the first frame update
    void Awake()
    {
        minePrefab = Resources.Load<GameObject>("Prefabs/Mine");
        fireRate = 3f;
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
