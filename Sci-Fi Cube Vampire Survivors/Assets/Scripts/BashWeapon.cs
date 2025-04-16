using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BashWeapon : Weapon
{
    private float bashDistance = 40f;
    private float bashDuration = 0.2f;
    private bool isBashing = false;

    private void Awake()
    {
        fireRate = 3f;
    }
    protected override void Attack()
    {
        StartCoroutine(Bash());
    }

    private IEnumerator Bash()
    {
        isBashing = true;
        GetComponentInParent<Player>().isDashing = true;

        Vector3 bashDirection = GetComponentInParent<Player>().direction.normalized;
        Vector3 startPosition = transform.position;

        float elapsed = 0f;

        while (elapsed < bashDuration)
        {
            float t = elapsed / bashDuration;
            float speed = Mathf.Sin(t * Mathf.PI * 2); // Smooth speed: 0 -> 1 -> 0

            float step = speed * bashDistance * Time.deltaTime * 50;

            transform.position += bashDirection * step;

            elapsed += Time.deltaTime;
            yield return null;
        }
        isBashing = false;
        GetComponentInParent<Player>().isDashing = false;
    }
}
