using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bash : WeaponAbstract
{
    [SerializeField] private float bashDistance = 5f;
    [SerializeField] private float bashDuration = 0.2f;
    private float TimeSinceBash = 0f;
    public float damage = 10f;

    

    protected override void Start()
    {
        fireRate = 2f;
        StartCoroutine(DelayBetweenWeapons());
    }

    private IEnumerator DelayBetweenWeapons()
    {
        foreach (var script in gameObject.GetComponents<Bash>())
        {
            yield return new WaitForSeconds(fireRate / 5);
        }
        base.Start();
    }

    private void Update()
    {
        TimeSinceBash += Time.deltaTime;
        HandleVisuals();
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

    private void HandleVisuals()
    {
        
        Color PlayerSpriteColour = GetComponentInChildren<SpriteRenderer>().color;
        Color.RGBToHSV(PlayerSpriteColour, out float h, out float s, out float v);
        v = (TimeSinceBash / fireRate) * 0.8f + 0.2f;
        PlayerSpriteColour = Color.HSVToRGB(h, s, v);
        GetComponentInChildren<SpriteRenderer>().color = PlayerSpriteColour;
    }

    private IEnumerator BashAttack()
    {
        // Set bashing to be true to enable the trail
        GetComponentInParent<Player>().isDashing = true;

        if (AudioManager.Instance != null) { AudioManager.Instance.PlaySFX("bashattack"); }


        // Get movement direction for the bash
        Vector3 bashDirection = GetComponentInParent<Player>().lastKnownDirection.normalized;

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
        GetComponentInParent<Player>().isDashing = false;
    }
    #endregion
}
