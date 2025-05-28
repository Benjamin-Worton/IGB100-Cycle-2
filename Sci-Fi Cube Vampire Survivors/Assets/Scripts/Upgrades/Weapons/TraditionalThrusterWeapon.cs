using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraditionalThruster : WeaponAbstract
{
    [SerializeField] private float bashDistance = 5f;
    [SerializeField] private float bashDuration = 0.2f;
    private float TimeSinceBash = 0f;
    public float damage = 50f;

    

    private void Awake()
    {
        fireRate = 3f;
    }

    private void Update()
    {
        TimeSinceBash += Time.deltaTime;
    }

    

    public override void Remove()
    {
        Destroy(this);
    }

    #region Attack Functions
    protected override void Attack()
        {
            StartCoroutine(BashAttack()); // Bash must be a Coroutine so call it instead of using Attack
        }

    private IEnumerator BashAttack()
        {
            GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in allEnemies)
            {
                if (Vector2.Distance(this.transform.position, enemy.transform.position) < 1) // Move all nearby enemies further away
                {
                    if (enemy.GetComponent<BasicEnemy>() != null)
                    {
                        enemy.GetComponent<BasicEnemy>().TakeDamage(damage, false);
                    }
                    if (enemy.GetComponent<Crate>() != null)
                    {
                        enemy.GetComponent<Crate>().TakeDamage();
                    }
                }
            }

            // Set bashing to be true to enable the trail
            GetComponentInParent<Player>().isThrusterDashing = true;

            // Get movement direction for the bash
            Vector3 bashDirection = GetComponentInParent<Player>().direction.normalized;

            // Elapsed tracks how long the bash has gone on for to match it to a sin wave
            float elapsed = 0f;
            while (elapsed < bashDuration)
            {
                float t = elapsed / bashDuration;
                float speed = Mathf.Sin(t * Mathf.PI);

                float step = speed * bashDistance * Time.deltaTime * 4;

                transform.position += bashDirection * step;

                elapsed += Time.deltaTime;
                yield return null;
            }
            TimeSinceBash = 0f;
            // Set bashing to be false to disable the trail
            GetComponentInParent<Player>().isThrusterDashing = false;
        }
    #endregion
}
