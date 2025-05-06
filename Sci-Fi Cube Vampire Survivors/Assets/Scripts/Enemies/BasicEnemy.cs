using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    // Stats
    public float maxHealth = 1f;
    private float CurrentHealth;
    public float speed = 5f;
    public int points = 100;

    public float pushBackForce = 1000f;
    public float damage = 10f;

    private GameObject target;
    private Rigidbody2D rb;
    private Player playerScript;

    private bool canMove = false;

    public GameObject ScrapPrefab;
    private int chanceOfDroppingScrap = 100; // In Percent %
    public float spawnHeight = 2f;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        playerScript = target.GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        CurrentHealth = maxHealth;

        StartCoroutine(StopMovementAfterDelay(1f));
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
        }

        if (canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime / 300f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canMove = false;

            if (collision.gameObject.GetComponent<Player>().isDashing)
            {
                TakeDamage(collision.gameObject.GetComponent<BashWeapon>().damage);
            }
            else
            {
                collision.gameObject.GetComponent<Player>().TakeDamage(damage);
            }

            Vector2 pushDirection = (transform.position - collision.transform.position).normalized;
            rb.velocity = Vector2.zero;
            rb.AddForce(pushDirection * pushBackForce, ForceMode2D.Impulse);

            StartCoroutine(StopMovementAfterDelay(0.5f));
        }

        if (collision.gameObject.CompareTag("Weapon"))
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage);
                if (bullet.destroyOnCollision) Destroy(collision.gameObject);
            }
        }
    }

    IEnumerator StopMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector2.zero;
        canMove = true;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            if (Random.Range(1, 101) <= chanceOfDroppingScrap)
            {
                StartCoroutine(SpawnScrapAndDie());
            }
            else
            {
                ScoreManager.instance.AddScore(points);
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator SpawnScrapAndDie()
    {
        int scrapCount = Random.Range(0, 3);
        float delayBetweenSpawns = 0.3f;

        for (int i = 0; i < scrapCount; i++)
        {
            SpawnScrap();
            yield return new WaitForSeconds(delayBetweenSpawns);
        }
        Player.instance.scrap += scrapCount;
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
