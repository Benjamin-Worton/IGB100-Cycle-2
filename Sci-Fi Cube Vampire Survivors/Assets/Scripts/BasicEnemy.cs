using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    // Stats
    public float maxHealth = 1;
    private float currentHealth;
    public float speed = 5;

    public float pushBackForce = 100f;

    private GameObject target;
    private Rigidbody2D rb;
    private Player playerScript;

    private bool canMove = false;

    void Start()
    {
        // Get the Rigidbody component of the player
        target = GameObject.FindGameObjectWithTag("Player");
        playerScript = target.GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();

        // Set the enemy to spawn with max health when spawned in
        currentHealth = maxHealth;

        // Since the enemies can spawn anywhere around the player
        // Dont allow them to move to the player has a chance to react
        StartCoroutine(StopMovementAfterDelay(1f)); // Wait for 1 second
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
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

            Vector3 direction = target.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the enemy is hit with a collider labeled "Weapon", take damage
        if (collision.gameObject.CompareTag("Weapon"))
        {
            currentHealth -= playerScript.damage;

            if (currentHealth <= 0)
            {
                Destroy(gameObject); // Destroy enemy
            }
        }

        // If the collider is labeled as player, deal damage
        // and push back the enemy away from the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Disable movement while pushing the enemy
            canMove = false;

            // Calculate the direction to push the enemy away from the player
            Vector2 pushDirection = (transform.position - collision.transform.position).normalized;
            rb.velocity = pushDirection * pushBackForce; // Apply pushback force

            // Stop the pushback after a short delay
            StartCoroutine(StopMovementAfterDelay(0.5f)); // You can adjust the delay if needed
        }
    }

    IEnumerator StopMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // After the delay, stop movement and allow the enemy to move again
        rb.velocity = Vector2.zero; // Stop the pushback velocity
        canMove = true; // Enable movement again
    }
}
