using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortableA35B : WeaponAbstract
{
    private GameObject rocketTargetPrefab;
    [SerializeField] private float damage = 100f;

    protected override void Attack()
    {
        for (int i = 0; i < 6; i++)
        {
            Vector3 randomPosition = Vector3.left * Random.Range(-6f, 6f) + Vector3.up * Random.Range(-3, 3);
            GameObject rocketTarget = Instantiate(rocketTargetPrefab, transform.position + randomPosition, Quaternion.identity); // Creates Mine and sets damage
            rocketTarget.GetComponent<RocketTarget>().damage = damage;
        }
        
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        rocketTargetPrefab = Resources.Load<GameObject>("Prefabs/Rocket Target");
        fireRate = 5f;
        StartCoroutine(DelayBetweenWeapons());
    }

    private IEnumerator DelayBetweenWeapons()
    {
        foreach(var script in gameObject.GetComponents<PortableA35B>())
        {
            yield return new WaitForSeconds(fireRate / 5);
        }
        base.Start();
    }

    public override void Remove()
    {
        Destroy(this);
    }
}
