using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
     private int round = 1;  // Starting round
    private int spawnedEnemies = 0;
    public float spawnInterval = 5f;
    public int numberRandomPositions = 10; // Total number of enmies to spawn in a round
    private int enemiesRemaining; 
    private int scoreCounter;

    public GameObject enemyPrefab;
    private GameObject player;
    public CircleCollider2D circleCollider;
    private BasicEnemy enemyScript;
    private Player playerScript;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerScript = player.GetComponent<Player>();
        }
        
        StartCoroutine(SpawnEnemies());
    }

    // This is made for a round based system, IDK if we are going to do this
    IEnumerator SpawnEnemies()
    {
         while (true)
        {
            spawnedEnemies = 0;
            enemiesRemaining = numberRandomPositions;

            while (spawnedEnemies < numberRandomPositions)
            {
                Vector2 spawnPos = RandomPointInCircle(circleCollider);
                GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

                // Increase enemy health each round (if BasicEnemy script exists)
                BasicEnemy enemyScript = enemy.GetComponent<BasicEnemy>();
                if (enemyScript != null)
                {
                    float healthBoost = enemyScript.maxHealth * 0.5f;
                    enemyScript.maxHealth += healthBoost;
                }

                spawnedEnemies++;
                enemiesRemaining--;

                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }

    // This works as a donut
    private Vector2 RandomPointInCircle(CircleCollider2D circle)
    {
        // Adjust for localScale.x (assuming uniform scale on X and Y)
        float scaledRadius = circle.radius * circle.transform.lossyScale.x;

        float minRadius = scaledRadius * 0.7f;
        float maxRadius = scaledRadius;

        float angle = Random.Range(0f, Mathf.PI * 2);
        float radius = Random.Range(minRadius, maxRadius);

        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        Vector2 center = (Vector2)circle.transform.position + circle.offset;

        return center + offset;
    }
}
