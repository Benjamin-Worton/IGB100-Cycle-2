using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{
    // Variables
    [HideInInspector] public Vector2 direction = Vector2.zero;
    [HideInInspector] public bool isDashing = false;
    private Rigidbody2D rb;
    public static Player instance; // Singleton pattern for easy access

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

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
    public int scrap = 0;
    public int maxInventorySpace = 1;
    public int currentInventorySpace = 1;

    public int exp = 0; 
    public int expNeeded = 20;
    public int level = 1;
    public float expSpeed = 100;

    // Outside objects
    public HealthBar healthBar;
    public EXPBar expBar;
    public TextMeshProUGUI levelText;

    // References
    [SerializeField] private TrailRenderer bashTrail;

    void Start()
    {
        CurrentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        exp = 0;
        expBar.SetMaxEXP(expNeeded);
        expBar.SetEXP(exp);
        levelText.text = "Level " + level.ToString();

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

        HandleExp();
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
        if (damage <= armour)
        {
            return;
        }
        CurrentHealth -= damage - armour;
        
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
            currentHealth *= 1.25f;
            if (currentHealth >= maxHealth)
            {
                currentHealth = maxHealth;
            }
            healthBar.SetHealth(currentHealth);
            Destroy(collision.gameObject);
        }
    }

    public void LevelUp()
    {
        level++;
        levelText.text = "Level " + level.ToString();

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
        expNeeded = expNeeded * 2;
        exp = 0;
        expBar.SetMaxEXP(expNeeded);
        expBar.SetEXP(exp);
        regen = regen * 2;
        speed = speed * 1.5f;

        Debug.Log("Player leveled up to level " + level);
    }

    private void HandleExp()
    {
        GameObject[] allExp = GameObject.FindGameObjectsWithTag("EXP");
        foreach (GameObject expObject in allExp)
        {
            float distanceToExp = Vector2.Distance(transform.position, expObject.transform.position);
            if (distanceToExp < 0.1f)
            {
                Destroy(expObject);
                exp += 1;
                expBar.SetEXP(exp);
            }
            if (exp >= expNeeded)
            {
                LevelUp();
            }
            if (distanceToExp < pickupRange)
            {
                Rigidbody2D expRB = expObject.GetComponent<Rigidbody2D>();
                Vector2 direction = (transform.position - expObject.transform.position).normalized;

                Vector2 velocity = expRB.velocity;
                if (velocity.magnitude == 0f)
                {
                    expRB.velocity = Vector2.up * expSpeed;
                    velocity = direction * expSpeed;
                }

                expRB.velocity = direction * velocity.magnitude * 1.01f;
            }
        }
    }
}
