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
    [Header("UI Stats")]
    private float currentHealth;
    public float maxHealth = 100f;
    public float CurrentHealth {
        get { return currentHealth; }
        set {
            currentHealth = value;
            healthBar.SetCurrent(currentHealth);
        }
    }
    public int exp = 0;
    public int EXP {
        get { return exp; }
        set {
            exp = value;
            expBar.SetCurrent(exp);
        }
    }
    private int expNeeded = 5;
    private int level = 1;
    private float expSpeed = 4;

    [Header("Player Stats")]
    public float regen = 0.1f; // Per Second
    public float armour = 0f; // Percent Reduction
    public float damageMultiplier = 1f;
    public float speed = 0.6f;
    
    private float pickupRange = 2f;
    public float CooldownMultiplier
    {
        get { return cooldownMultiplier; }
        set { 
            if (value < 0.05f) { cooldownMultiplier = 0.05f; }
            else { cooldownMultiplier = value; }
        }
    }
    private float cooldownMultiplier = 1f;

    public int scrap = 0;
    
    public float critRate = 0f;
    private float critDamageMultiplier = 1.5f;
    public float critBlock = 0f;
    private int mercilessStacks = 0;

    [Header("Inventory Stats")]
    public int maxInventorySpace = 1;
    public int currentInventorySpace = 1;






    // Outside objects
    [Header("Outside Objects")]
    [SerializeField] private Bar healthBar;
    [SerializeField] private Bar expBar;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TrailRenderer bashTrail;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private HurtEffect hurtEffect;

    void Start()
    {
        CurrentHealth = maxHealth;
        healthBar.SetMax(maxHealth);
        EXP = 0;
        expBar.SetMax(expNeeded);
        levelText.text = "Level " + level.ToString();

        // Setting Trail Width
        bashTrail.startWidth = 10/150f;
        rb = GetComponent<Rigidbody2D>();
        if (AudioManager.Instance != null) { AudioManager.Instance.PlayMusic("gameplaymusic1"); }

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

        HandleSprites();

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
            if (AudioManager.Instance != null) { AudioManager.Instance.PlaySFX("healthpickup"); } 
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
        expBar.SetMax(expNeeded);
        regen = regen * 2;
        speed = speed * 1.5f;
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
        StartCoroutine(hurtEffect.HurtFlash());
        return damage * (1f - armour);

    }
    private void HandleExp()
    {
        GameObject[] allExp = GameObject.FindGameObjectsWithTag("EXP");
        foreach (GameObject expObject in allExp)
        {
            float distanceToExp = Vector2.Distance(transform.position, expObject.transform.position);
            if (distanceToExp < 0.4f)
            {
                if (AudioManager.Instance != null) { AudioManager.Instance.PlaySFX("scrappickup"); }// Replace with XP pickup sound
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
                expRB.velocity = direction * expSpeed;
            }
        }
    }

    private void HandleSprites()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (direction.magnitude == 0f) { return; }
        if (direction.y != 0f) { sr.sprite = sprites[0]; }
        if (direction.x != 0f) { sr.sprite = sprites[1]; }
    }
}
