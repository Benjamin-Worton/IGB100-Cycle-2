using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public float damage = 5f;
    private GameObject ExplosionPrefab;
    [SerializeField] private float range = 2f;

    // Start is called before the first frame update
    void Start()
    {
        ExplosionPrefab = Resources.Load<GameObject>("Prefabs/Explosion");
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(2f);
        //Instantiate(ExplosionPrefab, transform.position, Quaternion.Identity);
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy"); // Find all nearby enemies, then damage and stun em
        if (allEnemies.Length != 0) {
            foreach (GameObject enemy in allEnemies)
            {
                if (Vector2.Distance(this.transform.position, enemy.transform.position) < range)
                {
                    if (enemy.GetComponent<BasicEnemy>() != null)
                    {
                        enemy.GetComponent<BasicEnemy>().Stun(1f);
                        enemy.GetComponent<BasicEnemy>().TakeDamage(damage);
                    }
                    if (enemy.GetComponent<Crate>() != null)
                    {
                        enemy.GetComponent<Crate>().TakeDamage();
                    }
                }
            }
        }
        GameObject Explosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(Explosion, 1f);
        Destroy(gameObject);
    }
}
