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

    public GameObject normalSprite;
    public GameObject whiteSprite;
    public float flashDuration = 2f;

    public float pushBackForce = 1000f;
    public float damage = 10f;

    private GameObject target;
    private Rigidbody2D rb;
    private Player playerScript;

    private bool canMove = false;

    public GameObject ScrapPrefab;
    private int chanceOfDroppingScrap = 100; // In Percent %

    private float burnTimeLeft = 0f;
    public GameObject expPrefab;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        playerScript = target.GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        CurrentHealth = maxHealth;

        normalSprite.SetActive(true);
        whiteSprite.SetActive(false);

        StartCoroutine(StopMovementAfterDelay(0.01f));
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
        }

        if (canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }

        if (target.transform.position.x + speed * Time.deltaTime < transform.position.x)
        {
            Vector3 scale = transform.localScale;
            scale.x = -1;
            transform.localScale = scale;
        }

        if (target.transform.position.x > transform.position.x + speed * Time.deltaTime)
        {
            Vector3 scale = transform.localScale;
            scale.x = 1;
            transform.localScale = scale;
        }

        if (burnTimeLeft > 0f) { TakeBurnDamage(); }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canMove = false;

            if (collision.gameObject.GetComponent<Player>().isDashing)
            {
                TakeDamage(collision.gameObject.GetComponent<Bash>().damage);
            }
            else
            {
                collision.gameObject.GetComponent<Player>().TakeDamage(damage);
                if (collision.gameObject.GetComponent<PlasmaShell>() != null)
                {
                    TakeDamage(collision.gameObject.GetComponent<PlasmaShell>().damage);
                }
            }

            Vector2 pushDirection = (transform.position - collision.transform.position).normalized;
            rb.velocity = Vector2.zero;
            rb.AddForce(pushDirection * pushBackForce, ForceMode2D.Impulse);

            StartCoroutine(StopMovementAfterDelay(0.5f));
        }

        if (collision.gameObject.CompareTag("Weapon"))
        {
#nullable enable
            Bullet? bullet;
            collision.gameObject.TryGetComponent<Bullet>(out bullet);
            if (bullet != null)
            {
                TakeDamage(bullet.damage);
                if (bullet.destroyOnCollision) Destroy(collision.gameObject);
            }
            IonLaserObject? ionLaser;
            collision.gameObject.TryGetComponent<IonLaserObject>(out ionLaser);
            if (ionLaser != null)
            {
                TakeDamage(ionLaser.damage);
            }
#nullable disable
        }
    }

    private IEnumerator StopMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector2.zero;
        canMove = true;
    }

    public void TakeDamage(float damage)
    {
        damage = playerScript.HandleDamageMultipliers(damage);
        CurrentHealth -= damage;

        StartCoroutine(DamageFlash());

        HandleOnTakeDamageEffects(damage);

        

        if (CurrentHealth <= 0 && CurrentHealth != -0.01f)
        {
            CurrentHealth = -0.01f; // Lock Health to prevent multiple deaths
            HandleOnDeathEffects();
            

            for (int i = 0; i < Random.Range(1, 6); i++)
            {
                SpawnExp();
            }

            if (Random.Range(1, 100) <= chanceOfDroppingScrap)
            {
                StartCoroutine(RandomScrap());
            }
            ScoreManager.instance.AddScore(points);
            Destroy(gameObject);
        }
    }

    private IEnumerator DamageFlash()
    {
        normalSprite.SetActive(false);
        whiteSprite.SetActive(true);

        yield return new WaitForSeconds(flashDuration);

        normalSprite.SetActive(true);
        whiteSprite.SetActive(false);
    }

    private IEnumerator RandomScrap()
    {
        int scrapCount = Random.Range(0, 3);
        float delayBetweenSpawns = 0.1f;
        for (int i = 0; i < scrapCount; i++)
        {
            SpawnScrap();
            yield return new WaitForSeconds(delayBetweenSpawns);
        }
        playerScript.scrap += scrapCount;
    }

    private IEnumerator DieAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private void SpawnScrap()
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

        Destroy(scrap, 0.5f);
    }

    private void SpawnExp()
    {
        // You can spawn multiple EXP or just one. Here's one for now:
        Vector3 spawnPosition = transform.position + Vector3.up * 0.2f;
        GameObject exp = Instantiate(expPrefab, spawnPosition, Quaternion.identity);

        Rigidbody2D rb = exp.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 forceDir = new Vector2(Random.Range(-1f, 1f), 1f).normalized;
            float forceMag = Random.Range(0.5f, 2f);
            rb.AddForce(forceDir * forceMag, ForceMode2D.Impulse);
            rb.AddTorque(Random.Range(-5f, 5f), ForceMode2D.Impulse);
        }

        // Optional: destroy the EXP pickup if not collected after some time
        Destroy(exp, 5f);
    }

    public void Stun(float Seconds)
    {
        if (!canMove) { return; }
        canMove = false;
        StartCoroutine(RemoveStun(Seconds));
    }

    private IEnumerator RemoveStun(float Seconds)
    {
        yield return new WaitForSeconds(Seconds);
        canMove = true;
    }


    private void HandleOnTakeDamageEffects(float damage)
    {
        // SteelEaters (Lifesteal)
        if (target.GetComponent<SteelEaters>() != null)
        {
            target.GetComponent<SteelEaters>().GiveHealth(damage);
        }

        // EMPRounds (Stun)
        if (target.GetComponent<EMPRounds>() != null)
        {
            Stun(1f);
        }

        // Incendiary Rounds
        if (target.GetComponent<IncendiaryRounds>() != null)
        {
            Ignite(5f);
        }
    }

    private void HandleOnDeathEffects()
    {
        // Merciless Programming (Decrease Cooldown On Kill)
        if (target.GetComponent<MercilessProgramming>() != null)
        {
            target.GetComponent<MercilessProgramming>().GiveMerciless();
        }

        // Handle Scrap Dropping
        if (Random.Range(1, 100) <= chanceOfDroppingScrap)
        {
            StartCoroutine(RandomScrap());
        }
        ScoreManager.instance.AddScore(points);
        canMove = false;
        StartCoroutine(DieAfterDelay());
    }

    // Burn Functions
    public void Ignite(float Seconds)
    {
        if (burnTimeLeft >= Seconds) { return; }
        burnTimeLeft = Seconds;
    }
    private void TakeBurnDamage()
    {
        if (CurrentHealth <= 0f) { return; }
        TakeDamage(Time.deltaTime * 10f);
        burnTimeLeft -= Time.deltaTime;
    }

}
