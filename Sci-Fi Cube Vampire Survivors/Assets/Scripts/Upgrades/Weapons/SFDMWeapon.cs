using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SFDM : WeaponAbstract
{
    private GameObject SFDMObjectPrefab;
    private GameObject SFDMObject;
    private float range = 3f;
    [SerializeField] private float damage = 5f;




    void Awake()
    {
        fireRate = 1f;  // Default fire rate

        // Set up weapon prefabs
        SFDMObjectPrefab = Resources.Load<GameObject>("Prefabs/SFDM Field");

        // Create SFDM Object above player's head
        SFDMObject = Instantiate(SFDMObjectPrefab, transform.position, Quaternion.identity);
    }
    void Update()
    {
        //Keep field locked to player
        SFDMObject.transform.position = transform.position;
    }

    public override void Remove()
    {
        Destroy(this);
    }

    #region Attack Functions
    protected override void Attack()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (allEnemies.Length == 0) { return; }
        foreach (GameObject enemy in allEnemies)
        {
            if (Vector2.Distance(this.transform.position, enemy.transform.position) < range) // Damage All nearby enemies
            {
                DamageEnemy(enemy);
            }
        }
    }

    private void DamageEnemy(GameObject enemy)
    {
        if (enemy.GetComponent<BasicEnemy>() != null)
        {
                enemy.GetComponent<BasicEnemy>().TakeDamage(damage);
        }
        if (enemy.GetComponent<Crate>() != null)
        {
            enemy.GetComponent<Crate>().TakeDamage();
        }

    }
    #endregion
}
