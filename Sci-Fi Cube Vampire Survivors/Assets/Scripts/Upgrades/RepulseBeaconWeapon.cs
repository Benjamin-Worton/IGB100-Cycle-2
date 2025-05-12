using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RepulseBeacon : Weapon
{
    private GameObject repulsePrefab;
    [SerializeField] private float DistanceFromPlayer = 3f;

    protected override void Attack()
    {
        //GameObject RepulseEffect = Instantiate(repulsePrefab, transform.position + Vector3.right * DistanceFromPlayer, Quaternion.identity);
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (allEnemies.Length != 0)
        {
            foreach (GameObject enemy in allEnemies)
            {
                if (Vector2.Distance(this.transform.position, enemy.transform.position) < DistanceFromPlayer) // Move all nearby enemies further away
                {
                    StartCoroutine(RepelEnemy(enemy.transform, transform.position + (enemy.transform.position - transform.position).normalized * DistanceFromPlayer));
                }
            }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        //repulsePrefab = Resources.Load<GameObject>("Prefabs/Laser");
        fireRate = 3f;
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
