using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RepulseBeacon : WeaponAbstract
{
    private GameObject repulsePrefab;
    [SerializeField] private float range = 3f;

    

    void Awake()
    {
        repulsePrefab = Resources.Load<GameObject>("Prefabs/Repulse");
        fireRate = 5f;
    }

    public override void Remove()
    {
        Destroy(this);
    }

    #region Attack Functions
    protected override void Attack()
    {
        GameObject RepulseEffect = Instantiate(repulsePrefab, transform.position, Quaternion.identity);
        Destroy(RepulseEffect, 0.5f);
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

    private IEnumerator RepelEnemy(Transform enemyTransform, Vector2 targetPosition)
    {
        Vector2 startPosition = enemyTransform.position;
        float elapsed = 0f;

        while (elapsed < 0.5f)
        {
            elapsed += Time.deltaTime;
            float t = elapsed * 2f;
            enemyTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        enemyTransform.position = targetPosition;
    }
    #endregion
}
