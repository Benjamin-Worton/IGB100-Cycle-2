using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private float OrbitDistance = 1f;

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
        for (int i = 0; i < total; i++)
        {
            WeaponAbstract weapon = orbitingWeapons[i];
            float angle = (360f / total) * i - 90f;
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;

            Vector3 orbitPos = (Vector2)playerTransform.position + direction * OrbitDistance;
            weapon.SetOrbitPosition(orbitPos);
        }
    }
}