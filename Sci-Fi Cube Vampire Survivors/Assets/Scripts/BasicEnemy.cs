using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private GameObject ScrapPrefab;
    private int chanceOfDroppingScrap = 100; // In Percent %

    void Start()
    {
        // Get the Rigidbody component of the player
        target = GameObject.FindGameObjectWithTag("Player");
        playerScript = target.GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();

        // Set the enemy to spawn with max health when spawned in
        CurrentHealth = maxHealth;

        // Since the enemies can spawn anywhere around the player
        // Dont allow them to move to the player has a chance to react
        StartCoroutine(StopMovementAfterDelay(1f)); // Wait for 1 second

        // Add the scrap prefab to the enemy for it to drop
        ScrapPrefab = Resources.Load<GameObject>("Prefabs/Scrap");
    }

    // Update is called once per frame
    void Update()
    {
        // If the player dies, despawn the enemies
        if (target == null)
        {
            Destroy(gameObject);
        }

        // Movement for the enemies
        // They will face and go towards the Player
        if (canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime / 300f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the collider is labeled as player, deal damage
        // and push back the enemy away from the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Disable movement while pushing the enemy
            canMove = false;

            if (collision.gameObject.GetComponent<Player>().isDashing)
            {
                TakeDamage(collision.gameObject.GetComponent<BashWeapon>().damage);
            }
            else
            {
                collision.gameObject.GetComponent<Player>().TakeDamage(damage);
            }

            // Calculate the direction to push the enemy away from the player
            Vector2 pushDirection = (transform.position - collision.transform.position).normalized;

            // Apply pushback force
            rb.velocity = Vector2.zero; // Clear any existing velocity first
            rb.AddForce(pushDirection * pushBackForce, ForceMode2D.Impulse); // Use impulse for immediate pushback

            // Stop the pushback after a short delay
            StartCoroutine(StopMovementAfterDelay(0.5f)); // You can adjust the delay if needed
        }
        
        if (collision.gameObject.CompareTag("Weapon")) {
            if (collision.gameObject.GetComponent<Bullet>() != null)
            {
                TakeDamage(collision.gameObject.GetComponent<Bullet>().damage);
                if (collision.gameObject.GetComponent<Bullet>().destroyOnCollision) Destroy(collision.gameObject);
            }
        }
    }

    IEnumerator StopMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // After the delay, stop movement and allow the enemy to move again
        rb.velocity = Vector2.zero; // Stop the pushback velocity
        canMove = true; // Enable movement again
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            if (Random.Range(1, 100) <= chanceOfDroppingScrap)
            {
                Instantiate(ScrapPrefab, transform.position, Quaternion.identity);
            }
            ScoreManager.instance.AddScore(points);
            Destroy(gameObject);
        }
    }
}
