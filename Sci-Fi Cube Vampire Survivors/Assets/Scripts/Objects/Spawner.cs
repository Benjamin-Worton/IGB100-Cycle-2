using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    private int round = 1;  // Starting round
    private int spawnedEnemies = 0;
    private int spawnedCrates = 0;
    public float spawnInterval = 10f;
    public int maximumSpawnAmount = 60;
    public int numberRandomPositions = 2; // Total number of enemies to spawn in a round
    private int enemiesRemaining; 
    private int scoreCounter;

    // New: Array of different enemy prefabs
    public GameObject[] enemyPrefabs;  // Array of possible enemy prefabs to spawn
    public GameObject cratePrefab;

    private GameObject player;
    public CircleCollider2D circleCollider;
    private Player playerScript;

    [SerializeField] private TutorialManager tutorialManager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerScript = player.GetComponent<Player>();
        }

        StartCoroutine(SpawnEnemies());
        StartCoroutine(IncreaseSpawnAmountOverTime()); // Start rate increase coroutine
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            spawnedEnemies = 0;
            enemiesRemaining = numberRandomPositions;

            StartCoroutine(tutorialManager.TutorialTip());

            while (spawnedEnemies < numberRandomPositions)
            {
                Vector2 spawnPos = RandomPointInCircle(circleCollider);

                // Randomly select an enemy prefab from the list
                GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, -0.5f);

                if (spawnedEnemies == 0 && round == 1)
                {
                    enemy.AddComponent<Target>();
                }

                BasicEnemy enemyScript = enemy.GetComponent<BasicEnemy>();
                if (enemyScript != null)
                {
                    // Scale enemy stats per round
                    float healthMultiplier = Mathf.Pow(1.2f, round - 1);
                    float speedMultiplier = Mathf.Pow(1.05f, round - 1);
                    float damageMultiplier = Mathf.Pow(1.2f, round - 1);

                    enemyScript.maxHealth *= healthMultiplier;
                    enemyScript.speed *= speedMultiplier;
                    enemyScript.damage *= damageMultiplier;
                }

                spawnedEnemies++;
                enemiesRemaining--;

                // Spawn crates randomly alongside enemies
                if (Random.Range(0f, 1f) <= 0.1f) // Adjust probability (e.g., 10% chance of spawning a crate)
                {

                    Vector2 crateSpawnPos = RandomPointInCircle(circleCollider);  // Random spawn position for crate
                    Instantiate(cratePrefab, crateSpawnPos, Quaternion.identity);

                    if (spawnedCrates == 0)
                    {
                        cratePrefab.AddComponent<GoodTarget>();
                        spawnedCrates++;
                    }
                }
                yield return new WaitForSeconds(spawnInterval);
            }
            
        }
    }

    IEnumerator IncreaseSpawnAmountOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(20f); // Wait 20 seconds
            numberRandomPositions = (int)Mathf.Ceil(numberRandomPositions * 2f);
            if (numberRandomPositions > maximumSpawnAmount)
                numberRandomPositions = maximumSpawnAmount;

            round++;
        }   
    }

    private Vector2 RandomPointInCircle(CircleCollider2D circle)
    {
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