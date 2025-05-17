using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornetDrones : WeaponAbstract
{
    private GameObject dronePrefab;
    private readonly float DistanceFromPlayer = 0.5f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 5f;
    private GameObject RightDrone;
    private GameObject LeftDrone;

    protected override void Attack()
    {
        RightDrone.GetComponent<HornetDrone>().Fire();
        LeftDrone.GetComponent<HornetDrone>().Fire();
    }

    void Awake()
    {
        fireRate = 2f; // Default fire rate
        dronePrefab = Resources.Load<GameObject>("Prefabs/HornetDrone");


        // Create and position drones, as well as modify their stats
        RightDrone = Instantiate(dronePrefab, transform.position + Vector3.right * DistanceFromPlayer + Vector3.up * 0.8f, Quaternion.identity);
        LeftDrone = Instantiate(dronePrefab, transform.position + Vector3.right * -DistanceFromPlayer + Vector3.up * 0.8f, Quaternion.identity);
        RightDrone.GetComponent<HornetDrone>().damage = damage;
        LeftDrone.GetComponent<HornetDrone>().damage = damage;
        RightDrone.GetComponent<HornetDrone>().range = range;
        LeftDrone.GetComponent<HornetDrone>().range = range;
    }

    private void Update()
    {
        // Keep drones locked to player
        RightDrone.transform.position = transform.position + Vector3.right * DistanceFromPlayer + Vector3.up * 0.8f;
        LeftDrone.transform.position = transform.position + Vector3.right * -DistanceFromPlayer + Vector3.up * 0.8f;
    }

    public override void Remove()
    {
        Destroy(this);
    }
}