using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornetDrones : Weapon
{
    private GameObject dronePrefab;
    private readonly float DistanceFromPlayer = 0.5f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 3.5f;
    private GameObject RightDrone;
    private GameObject LeftDrone;

    protected override void Attack()
    {
        RightDrone.GetComponent<HornetDrone>().Fire();
        LeftDrone.GetComponent<HornetDrone>().Fire();
    }

    // Start is called before the first frame update
    void Awake()
    {
        fireRate = 2f; // Default fire rate
        dronePrefab = Resources.Load<GameObject>("Prefabs/HornetDrone");

        RightDrone = Instantiate(dronePrefab, transform.position + Vector3.right * DistanceFromPlayer + Vector3.up * 0.2f, Quaternion.identity);
        LeftDrone = Instantiate(dronePrefab, transform.position + Vector3.right * -DistanceFromPlayer + Vector3.up * 0.2f, Quaternion.identity);
        RightDrone.GetComponent<HornetDrone>().damage = damage;
        LeftDrone.GetComponent<HornetDrone>().damage = damage;
        RightDrone.GetComponent<HornetDrone>().range = range;
        LeftDrone.GetComponent<HornetDrone>().range = range;
    }

    private void Update()
    {
        RightDrone.transform.position = transform.position + Vector3.right * DistanceFromPlayer + Vector3.up * 0.8f;
        LeftDrone.transform.position = transform.position + Vector3.right * -DistanceFromPlayer + Vector3.up * 0.8f;
    }

    public override void Remove()
    {
        Destroy(this);
    }

    // Method to upgrade fire rate (called by UpgradeMenu)
    public void UpgradeFireRate()
    {
        fireRate /= 1.5f;  // Increase fire rate (faster firing)
    }
}