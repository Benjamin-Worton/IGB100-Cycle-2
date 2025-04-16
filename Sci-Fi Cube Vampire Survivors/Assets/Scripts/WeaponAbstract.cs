using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    // Variables For All Weapons
    [SerializeField] protected float fireRate;
    protected GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<GameObject>();
        StartCoroutine(FireLoop());
    }

    private IEnumerator FireLoop()
    {
        while (true)
        {
            Attack();
            if (fireRate == 0) break;
            yield return new WaitForSeconds(fireRate);
        }
    }

    protected abstract void Attack();
}
