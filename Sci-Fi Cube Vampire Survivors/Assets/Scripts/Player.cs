using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;
using TMPro;

public class Player : MonoBehaviour
{
    // Variables
    [HideInInspector] public Vector2 direction = Vector2.zero;
    [HideInInspector] public bool isDashing = false;
    [HideInInspector] public bool isThrusterDashing = false;
    private Rigidbody2D rb;
    [HideInInspector] public Vector2 lastKnownDirection = Vector2.zero;


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
    public float armour = 0f; // Percent Reduction
    public float damageMultiplier = 1f;
    public float speed = 0.3f;
    public float maxHealth = 100f;
    public float pickupRange = 1f;
    public float CooldownMultiplier
    {
        get { return cooldownMultiplier; }
        set { 
            if (value < 0.05f) { cooldownMultiplier = 0.05f; }
            else { cooldownMultiplier = value; }
        }
    }
    public float cooldownMultiplier = 1f;

    public int scrap = 0;
    public int maxInventorySpace = 1;
    public int currentInventorySpace = 1;
    public float critRate = 0f;
    public float critDamageMultiplier = 1.5f;
    public float critBlock = 0f;


    private int mercilessStacks = 0;

    public int exp = 0;
    public int EXP {
        get { return exp; }
        set {
            exp = value;
            expBar.SetEXP(exp);
        }
    }

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
        EXP = 0;
        expBar.SetMaxEXP(expNeeded);
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
            rb.MovePosition(rb.position + direction.normalized * speed * Time.fixedDeltaTime / 0.6f);
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
        if (isThrusterDashing) { isDashing = true; }

        HandleExp();
    }
    private void HandleSpeed()
    {
        speed = 1f;

        foreach(var script in GetComponents<MovementUpgradeAbstract>())
        {
            speed = script.HandleSpeed(speed);
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
        EXP = 0;
        expBar.SetMaxEXP(expNeeded);
        regen = regen * 2;
        speed = speed * 1.5f;

        Debug.Log("Player leveled up to level " + level);
    }

    public void GiveMerciless()
    {
        if (mercilessStacks >= 3) { return; }
        mercilessStacks++;
        CooldownMultiplier -= 0.1f;
        StartCoroutine(RemoveMerciless());
    }

    private IEnumerator RemoveMerciless()
    {
        yield return new WaitForSeconds(1);
        CooldownMultiplier += 0.1f;
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
    private void HandleExp()
    {
        GameObject[] allExp = GameObject.FindGameObjectsWithTag("EXP");
        foreach (GameObject expObject in allExp)
        {
            float distanceToExp = Vector2.Distance(transform.position, expObject.transform.position);
            if (distanceToExp < 0.5f)
            {
                Destroy(expObject);
                EXP += 1;
            }
            if (EXP >= expNeeded)
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
                    expRB.velocity = Vector2.up * expSpeed * 0.1f;
                    velocity = direction * expSpeed;
                }

                expRB.velocity = direction * velocity.magnitude * 1.01f;
            }
        }
    }
}
