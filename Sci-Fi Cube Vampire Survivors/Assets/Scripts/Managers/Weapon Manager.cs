using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private float OrbitDistance = 1f;
    private float angleBonus = 0f;
    private List<WeaponAbstract> orbitingWeapons = new List<WeaponAbstract>();
    private Transform playerTransform;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        StartCoroutine(DelayedRefresh());
    }

    private void Update()
    {
        UpdateOrbitPositions();
    }

    public void AddWeapon(WeaponAbstract weapon)
    {
        if (weapon is IOrbiting && !orbitingWeapons.Contains(weapon))
        {
            orbitingWeapons.Add(weapon);
        }
    }

    public void RemoveWeapon(WeaponAbstract weapon)
    {
        orbitingWeapons.Remove(weapon);
    }

    public void RefreshOrbitingWeapons()
    {
        orbitingWeapons.Clear();
        WeaponAbstract[] allWeapons = playerTransform.GetComponents<WeaponAbstract>();

        foreach (var script in allWeapons)
        {
            if (script is IOrbiting)
            {
                orbitingWeapons.Add(script);
            }
        }
    }

    private IEnumerator DelayedRefresh()
    {
        yield return new WaitForEndOfFrame();
        RefreshOrbitingWeapons();
    }

    private void UpdateOrbitPositions()
    {
        int total = orbitingWeapons.Count;
        angleBonus += Time.deltaTime * 10f;
        float ringSpacing = 1f;
        int currentIndex = 0;
        int ring = 1;

        while (currentIndex < total)
        {
            int weaponsInCurrentRing = ring * 8;
            float radius = OrbitDistance + (ring - 1) * ringSpacing;
            for (int i= 0; i < weaponsInCurrentRing && currentIndex < total; i++, currentIndex++)
            {
                WeaponAbstract weapon = orbitingWeapons[currentIndex];
                float angle = (360f / weaponsInCurrentRing) * i + 90f + angleBonus;
                Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
                Vector3 orbitPos = (Vector2)playerTransform.position + direction * radius;
                weapon.SetOrbitPosition(orbitPos);
            }
            ring++;
        }
    }
}