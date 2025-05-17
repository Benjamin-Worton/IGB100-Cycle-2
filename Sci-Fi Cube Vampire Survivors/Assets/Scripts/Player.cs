using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    // Variables
    [HideInInspector] public Vector2 direction = Vector2.zero;
    [HideInInspector] public bool isDashing = false;
    private Rigidbody2D rb;
    [HideInInspector] public Vector2 lastKnownDirection = Vector2.zero;


    // Stats
    public int level = 1;
    public float currentHealth;
    public float CurrentHealth {
        get { return currentHealth; }
        set { 
            currentHealth = value;
            healthBar.SetHealth(currentHealth);
        }
    }
    public float regen = 0.1f; // Per Second
    public float armour = 0f; // Percent Reduction
    public float damageMultiplier = 1f;
    public float speed = 0.3f;
    public float maxHealth = 100f;
    public float pickupRange = 50f;
    public float cooldownMultiplier = 1f; // Percent, AVOID 0% MOST LIKELY WILL BREAK GAME
    public int scrap = 0;
    public int maxInventorySpace = 1;
    public int currentInventorySpace = 1;
    public float critRate = 0f;
    public float critDamageMultiplier = 1.5f;
    public float critBlock = 0f;


    private int mercilessStacks = 0;

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
            lastKnownDirection = direction;
        }
        if (CurrentHealth < maxHealth)
        {
            CurrentHealth += regen / 50f;
            if (CurrentHealth >= maxHealth)
            {
                CurrentHealth = maxHealth;
            }
        }
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
        if (gameObject.GetComponent<HoverPad>() != null)
        {
            speed = gameObject.GetComponent<HoverPad>().HandleSpeed(speed);
        }
    }

    public void TakeDamage(float damage)
    {
        damage = HandleDamageTaken(damage);
        CurrentHealth -= damage * (1f - armour);
        
        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        SceneManager.LoadScene("Lose Screen");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Health"))
        {
            ScoreManager.instance.AddScore(200);
            CurrentHealth *= 1.25f;
            Destroy(collision.gameObject);
        }
    }

    public void LevelUp()
    {
        level++;

        // Inventory space progression
        switch (level)
        {
            case 2:
                maxInventorySpace = 2;
                break;
            case 3:
                maxInventorySpace = 4;
                break;
            case 4:
                maxInventorySpace = 8;
                break;
            case 5:
                maxInventorySpace = 9;
                break;
            default:
                break;
        }

        currentInventorySpace = maxInventorySpace;

        Debug.Log("Player leveled up to level " + level);
    }

    public void GiveMerciless()
    {
        if (mercilessStacks >= 3) { return; }
        mercilessStacks++;
        cooldownMultiplier -= 0.1f;
        StartCoroutine(RemoveMerciless());
    }

    private IEnumerator RemoveMerciless()
    {
        yield return new WaitForSeconds(1);
        cooldownMultiplier += 0.1f;
        mercilessStacks--;
    }

    public float HandleDamageMultipliers(float damage)
    {
        if (Random.Range(1, 100) <= critRate * 100)
        {
            damage *= critDamageMultiplier;
        }

        return (damage * damageMultiplier);
    }

    private float HandleDamageTaken(float damage)
    {
        if (Random.Range(1, 100) <= critBlock * 100)
        {
            return 0;
        }
        return damage * (1f - armour);
    }
}
