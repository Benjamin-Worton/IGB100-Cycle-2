using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    private GameObject target;
    private Player playerScript;
    private Rigidbody2D rb;

    public GameObject ScrapPrefab;
    private GameObject healthPrefab;

    private int chanceOfDroppingScrap = 100; // In Percent %
    public int points = 500;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Rigidbody component of the player
        target = GameObject.FindGameObjectWithTag("Player");
        playerScript = target.GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();

        healthPrefab = Resources.Load<GameObject>("Prefabs/Health");
        
    }

    // Update is called once per frame
    void Update()
    {
        // If the player dies, despawn the enemies
        if (target == null)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the collider is labeled as player, deal damage
        // and push back the enemy away from the player
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<Player>().isDashing)
            {
                TakeDamage();
            }
        }
        
        if (collision.gameObject.CompareTag("Weapon")) {
            if (collision.gameObject.GetComponent<Bullet>() != null)
            {
                TakeDamage();
                if (collision.gameObject.GetComponent<Bullet>().destroyOnCollision) Destroy(collision.gameObject);
            }
        }
    }

    public void TakeDamage()
    {
            if (Random.Range(1, 100) <= chanceOfDroppingScrap)
            {
                StartCoroutine(RandomScrap());
            }
            Instantiate(healthPrefab, transform.position, Quaternion.identity);
            ScoreManager.instance.AddScore(points);
            Destroy(gameObject);
    }

    private IEnumerator RandomScrap()
    {
        int scrapCount = Random.Range(0, 3);
        float delayBetweenSpawns = 0.3f;

        for (int i = 0; i < scrapCount; i++)
        {
            SpawnScrap();
            yield return new WaitForSeconds(delayBetweenSpawns);
        }
        playerScript.scrap += scrapCount;
        ScrapCounter.instance.AddScrap(scrapCount);

        ScoreManager.instance.AddScore(points);
        Destroy(gameObject);
    }

    void SpawnScrap()
    {
        // Spawn just above the enemy
        Vector3 spawnPosition = transform.position + Vector3.up * 0.5f;

        // Instantiate the scrap prefab
        GameObject scrap = Instantiate(ScrapPrefab, spawnPosition, Quaternion.identity);

        // Apply arcing force
        Rigidbody2D scrapRb = scrap.GetComponent<Rigidbody2D>();
        if (scrapRb != null)
        {
            // Randomize arc direction a little
            Vector2 forceDirection = new Vector2(Random.Range(-1f, 1f), 1f).normalized;
            float forceMagnitude = Random.Range(1f, 6f); // adjust this to control the arc height/distance

            scrapRb.AddForce(forceDirection * forceMagnitude, ForceMode2D.Impulse);
        }
        // Applying Spin on scrap
        scrapRb.AddTorque(Random.Range(-10f, 10f), ForceMode2D.Impulse);
    }
}
