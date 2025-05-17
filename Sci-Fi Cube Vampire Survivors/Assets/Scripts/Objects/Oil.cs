using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Oil : MonoBehaviour
{
    [HideInInspector] public float burnTime = 5f;
    [HideInInspector] public float burnDistance = 1.5f;
    private bool isIgnited = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            Ignite();
        }
    }

    public void Ignite()
    {
        // Check if already ignited, if not, ignite
        if (isIgnited) { return; }
        isIgnited = true;
        IgniteNearbyOil();
        IgniteNearbyEnemies();
        Destroy(gameObject);
    }

    private void IgniteNearbyOil()
    {
        // Get all oil, then ignite nearby oil
        GameObject[] allOil = GameObject.FindGameObjectsWithTag("Oil");
        if (allOil.Length == 0) { return; }

        foreach (GameObject oil in allOil)
        {
            if (Vector2.Distance(oil.transform.position, transform.position) >= burnDistance || oil == gameObject) { continue; }
            oil.GetComponent<Oil>().Ignite();
        }
    }

    private void IgniteNearbyEnemies()
    {
        // Get all enemies, then ignite nearby enemies
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (allEnemies.Length == 0) { return; }

        foreach (GameObject enemies in allEnemies)
        {
            if (Vector2.Distance(enemies.transform.position, transform.position) >= burnDistance || enemies == gameObject) { continue; }
            enemies.GetComponent<BasicEnemy>().Ignite(burnTime);
        }
    }
}
