using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTarget : MonoBehaviour
{
    public float damage = 5f;
    private GameObject ExplosionPrefab;
    private GameObject rocketPrefab;
    private GameObject rocket;
    [SerializeField] private float range = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        // ExplosionPrefab = Resources.Load<GameObject>("Prefabs/Explosion"); Implement when explosion created
        rocketPrefab = Resources.Load<GameObject>("Prefabs/Rocket");
        rocket = Instantiate(rocketPrefab, transform.position + Vector3.up * 10f, Quaternion.identity);
        StartCoroutine(MoveRocketDown());
    }

    private IEnumerator MoveRocketDown()
    {
        Vector3 startPos = rocket.transform.position;
        Vector3 endPos = transform.position;
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            rocket.transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rocket.transform.position = endPos;
        Explode();
    }

    private void Explode()
    {
        //Instantiate(ExplosionPrefab, transform.position, Quaternion.Identity);
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy"); // Find all nearby enemies, then damage and stun em
        if (allEnemies.Length != 0)
        {
            foreach (GameObject enemy in allEnemies)
            {
                if (Vector2.Distance(this.transform.position, enemy.transform.position) < range)
                {
                    if (enemy.GetComponent<BasicEnemy>() != null)
                    {
                        enemy.GetComponent<BasicEnemy>().TakeDamage(damage);
                    }
                    if (enemy.GetComponent<Crate>() != null)
                    {
                        enemy.GetComponent<Crate>().TakeDamage();
                    }

                }
            }
        }

        Destroy(rocket);
        Destroy(gameObject);
    }
}
