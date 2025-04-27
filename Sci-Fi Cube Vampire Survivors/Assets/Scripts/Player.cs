using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Variables
    [HideInInspector] public Vector2 direction = Vector2.zero;
    [HideInInspector] public bool isDashing = false;
    private Rigidbody2D rb;
    private float scrapSpeed = 3f;

    // Stats
    public float currentHealth;
    public float CurrentHealth {
        get { return currentHealth; }
        set { 
            currentHealth = value;
            healthBar.SetHealth(currentHealth);
        }
    }
    public float regen = 0.1f; // Per Second
    public float armour = 0f;
    public float speed = 1f;
    public float maxHealth = 100f;
    public float pickupRange = 50f;
    public int scrap = 10;

    // Outside objects
    public HealthBar healthBar;

    // References
    [SerializeField] private TrailRenderer bashTrail;

    void Start()
    {
        CurrentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        // Setting Trail Width
        bashTrail.startWidth = 10/150f;
        rb = GetComponent<Rigidbody2D>();

    }

    private void FixedUpdate()
    {
        if (direction.magnitude != 0f)
        {
            HandleSpeed();
            // Move Main Scene (Player + Camera)
            rb.MovePosition(rb.position + direction.normalized * speed * Time.fixedDeltaTime);
        }
        if (CurrentHealth < maxHealth)
        {
            CurrentHealth += regen / 60f;
            if (CurrentHealth >= maxHealth)
            {
                CurrentHealth = maxHealth;
            }
        }
        HandleScrap();
    }

    void Update()
    {
        // Player Movement
        direction = Vector2.zero;
        // Set Directions
        if (Input.GetKey(KeyCode.W)) direction.y += 1f;
        if (Input.GetKey(KeyCode.S)) direction.y -= 1f;
        if (Input.GetKey(KeyCode.A)) direction.x -= 1f;
        if (Input.GetKey(KeyCode.D)) direction.x += 1f;

        // If Dashing (set by Bash) then turn on trail
        bashTrail.emitting = isDashing;
    }
    private void HandleSpeed()
    {
        speed = 1f;

        if (gameObject.GetComponent<RoboticTracks>() != null)
        {
            speed = gameObject.GetComponent<RoboticTracks>().HandleSpeed(speed);
        }
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        
        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        SceneManager.LoadScene("Lose Screen");
    }

    private void HandleScrap()
    {
        GameObject[] allScrap = GameObject.FindGameObjectsWithTag("Scrap");
        foreach (GameObject scrapObject in allScrap)
        {
            float distanceToScrap = Vector2.Distance(transform.position, scrapObject.transform.position);
            if (distanceToScrap < 0.1f)
            {
                Destroy(scrapObject);
                scrap += 1;
            }
            if (distanceToScrap < pickupRange)
            {
                Rigidbody2D scrapRB = scrapObject.GetComponent<Rigidbody2D>();
                Vector2 direction = (transform.position - scrapObject.transform.position).normalized;

                Vector2 velocity = scrapRB.velocity;
                if (velocity.magnitude == 0f)
                {
                    scrapRB.velocity = Vector2.up * scrapSpeed;
                    velocity = direction * scrapSpeed;
                }

                scrapRB.velocity = direction * velocity.magnitude * 1.01f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Health"))
        {
            ScoreManager.instance.AddScore(200);
            currentHealth *= 1.25f;
            if (currentHealth >= maxHealth)
            {
                currentHealth = maxHealth;
            }
            healthBar.SetHealth(currentHealth);
            Destroy(collision.gameObject);
        }
    }

}
