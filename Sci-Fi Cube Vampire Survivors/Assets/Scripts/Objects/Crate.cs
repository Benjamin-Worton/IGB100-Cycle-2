using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    private GameObject target;
    private Player playerScript;
    private Rigidbody2D rb;

    private GameObject ScrapPrefab;
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

        ScrapPrefab = Resources.Load<GameObject>("Prefabs/Scrap");
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
                Instantiate(ScrapPrefab, transform.position, Quaternion.identity);
            }
            Instantiate(healthPrefab, transform.position, Quaternion.identity);
            ScoreManager.instance.AddScore(points);
            Destroy(gameObject);
    }
}
