using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BashWeapon : Weapon
{
    [SerializeField] private float bashDistance = 40f;
    [SerializeField] private float bashDuration = 0.2f;

    protected override void Attack()
    {
        StartCoroutine(Bash()); // Bash must be a Coroutine so call it instead of using Attack
    }

    private void Awake()
    {
        // ADD FIRERATE WHEN PAST TESTING
    }

    private IEnumerator Bash()
    {
        // Set bashing to be true to enable the trail
        GetComponentInParent<Player>().isDashing = true;

        // Get movement direction for the bash
        Vector3 bashDirection = GetComponentInParent<Player>().direction.normalized;

        // Elapsed tracks how long the bash has gone on for to match it to a sin wave
        float elapsed = 0f;
        while (elapsed < bashDuration)
        {
            float t = elapsed / bashDuration;
            float speed = Mathf.Sin(t * Mathf.PI * 2);

            float step = speed * bashDistance * Time.deltaTime * 50;

            transform.position += bashDirection * step;

            elapsed += Time.deltaTime;
            yield return null;
        }
        // Set bashing to be false to disable the trail
        GetComponentInParent<Player>().isDashing = false;
        transform.position = this.transform.parent.position;
    }
}
