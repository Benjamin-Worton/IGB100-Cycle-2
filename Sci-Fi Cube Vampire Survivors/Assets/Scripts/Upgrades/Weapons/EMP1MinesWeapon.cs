using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP1Mines : WeaponAbstract
{
    private GameObject minePrefab;
    [SerializeField] private float damage = 50f;

    protected override void Attack()
    {
        GameObject Mine = Instantiate(minePrefab, transform.position + Vector3.forward * 0.1f, Quaternion.identity); // Creates Mine and sets damage
        Mine.GetComponent<Mine>().damage = damage;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        minePrefab = Resources.Load<GameObject>("Prefabs/Mine");
        fireRate = 3f;
        StartCoroutine(DelayBetweenWeapons());
    }

    private IEnumerator DelayBetweenWeapons()
    {
        foreach(var script in gameObject.GetComponents<EMP1Mines>())
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
