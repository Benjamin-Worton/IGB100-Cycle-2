using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    private int round = 1;  // Starting round
    private int spawnedEnemies = 0;
    public float spawnInterval = 5f;
    public float minimumSpawnInterval = 0.5f; // Optional: prevent interval from getting too small
    public int numberRandomPositions = 10; // Total number of enemies to spawn in a round
    private int enemiesRemaining; 
    private int scoreCounter;
    public float roundPause = 5f;

    // New: Array of different enemy prefabs
    public GameObject[] enemyPrefabs;  // Array of possible enemy prefabs to spawn
    public GameObject cratePrefab;

    private GameObject player;
    public CircleCollider2D circleCollider;
    private Player playerScript;
    public TMP_Text roundText;
    public Slider enemiesRemainingSlider;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerScript = player.GetComponent<Player>();
        }

        StartCoroutine(SpawnEnemies());
        StartCoroutine(IncreaseSpawnRateOverTime()); // Start rate increase coroutine
        StartCoroutine(DrainSliderOverTime());
        roundText.text = "Round: " + round.ToString();  // Update the text component to show the current round
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            spawnedEnemies = 0;
            enemiesRemaining = numberRandomPositions;

            while (spawnedEnemies < numberRandomPositions)
            {
                Vector2 spawnPos = RandomPointInCircle(circleCollider);

                // Randomly select an enemy prefab from the list
                GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

                // gives a singlular enemy a 'target' 
                if (spawnedEnemies == 0)
                {
                    enemy.AddComponent<Target>();
                }

                BasicEnemy enemyScript = enemy.GetComponent<BasicEnemy>();
                if (enemyScript != null)
                {
                    // Scale enemy stats per round
                    float healthMultiplier = Mathf.Pow(1.5f, round - 1);
                    float speedMultiplier = Mathf.Pow(1.5f, round - 1);
                    float damageMultiplier = Mathf.Pow(1.5f, round - 1);

                    enemyScript.maxHealth *= healthMultiplier;
                    enemyScript.speed *= speedMultiplier;
                    enemyScript.damage *= damageMultiplier;
                }

                // gives time for new players to kill their first basic enemy before the swarm comes
                if (spawnedEnemies == 0)
                {
                    yield return new WaitForSeconds(roundPause);
                }

                spawnedEnemies++;
                enemiesRemaining--;

                // Spawn crates randomly alongside enemies
                if (Random.Range(0f, 1f) <= 0.1f) // Adjust probability (e.g., 10% chance of spawning a crate)
                {
                    Vector2 crateSpawnPos = RandomPointInCircle(circleCollider);  // Random spawn position for crate
                    Instantiate(cratePrefab, crateSpawnPos, Quaternion.identity);
                }

                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }

    IEnumerator IncreaseSpawnRateOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(60f); // Wait 60 seconds
            spawnInterval *= 0.75f;
            if (spawnInterval < minimumSpawnInterval)
                spawnInterval = minimumSpawnInterval;

            round++;
            roundText.text = "Round: " + round.ToString();  // Update the text component to show the current round

            if (playerScript != null)
            {
                playerScript.LevelUp();
            }
        }   
    }

    IEnumerator DrainSliderOverTime()
    {
        while (true)
        {
            enemiesRemainingSlider.maxValue = 60f;
            enemiesRemainingSlider.value = 60f;

            float duration = 60f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                enemiesRemainingSlider.value = Mathf.Lerp(60f, 0f, elapsed / duration);
                yield return null;
            }

            enemiesRemainingSlider.value = 0f;
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