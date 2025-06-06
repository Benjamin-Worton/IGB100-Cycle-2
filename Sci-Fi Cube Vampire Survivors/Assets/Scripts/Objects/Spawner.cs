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
    private float spawnInterval = 2f;
    private int maximumSpawnAmount = 60;
    private float numberRandomPositions = 1; // Total number of enemies to spawn in a round
    private int enemiesRemaining; 
    private int scoreCounter;

    // New: Array of different enemy prefabs
    public GameObject[] enemyPrefabs;  // Array of possible enemy prefabs to spawn
    public GameObject cratePrefab;

    private GameObject player;
    public CircleCollider2D circleCollider;
    private Player playerScript;

    [SerializeField] private TutorialManager tutorialManager;
    private const string tutorialPrefKey = "ShowTutorial";

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerScript = player.GetComponent<Player>();
        }

        if (PlayerPrefs.GetInt(tutorialPrefKey, 1) == 1)
        {
            StartCoroutine(tutorialManager.TutorialTip());
        }
        else
        {
            tutorialManager.NoIntro();
        }
        
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnCrates());
        StartCoroutine(IncreaseSpawnAmountOverTime()); // Start rate increase coroutine
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            spawnedEnemies = 0;

            while (spawnedEnemies < Mathf.Floor(numberRandomPositions))
            {
                Vector2 spawnPos = RandomPointInCircle(circleCollider);

                // Randomly select an enemy prefab from the list
                GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, -0.5f);

                if (spawnedEnemies == 0 && round == 1 && enemy.GetComponent<Target>() == null)
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
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    
    IEnumerator SpawnCrates()
    {
        while (true)
        {
            while (spawnedCrates < numberRandomPositions)
            {
                Vector2 spawnPos = RandomPointInCircle(circleCollider);

                if (Random.Range(0f, 1f) <= 0.1f)
                {
                    Vector2 crateSpawnPos = RandomPointInCircle(circleCollider);
                    GameObject crate = Instantiate(cratePrefab, crateSpawnPos, Quaternion.identity);
                    if (crate.GetComponent<GoodTarget>() == null) { crate.AddComponent<GoodTarget>(); }
                    spawnedCrates++;
                }

                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }


    IEnumerator IncreaseSpawnAmountOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f); // Wait 20 seconds
            numberRandomPositions += 0.5f;
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