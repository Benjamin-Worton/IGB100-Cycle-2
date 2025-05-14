using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RepulseBeacon : Weapon
{
    private GameObject repulsePrefab;
    [SerializeField] private float range = 3f;

    protected override void Attack()
    {
        //GameObject RepulseEffect = Instantiate(repulsePrefab, transform.position, Quaternion.identity);
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (allEnemies.Length == 0) { return; }
        foreach (GameObject enemy in allEnemies)
        {
            if (Vector2.Distance(this.transform.position, enemy.transform.position) < range) // Move all nearby enemies further away
            {
                StartCoroutine(RepelEnemy(enemy.transform, transform.position + (enemy.transform.position - transform.position).normalized * range));
            }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        //repulsePrefab = Resources.Load<GameObject>("Prefabs/Laser");
        fireRate = 8f;
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

    private IEnumerator RepelEnemy(Transform enemyTransform, Vector2 targetPosition)
    {
        Vector2 startPosition = enemyTransform.position;
        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime;
            float t = elapsed;
            enemyTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        enemyTransform.position = targetPosition;
    }
}
