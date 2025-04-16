using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// A basic abstract class that is given to all weapons to save some time and allow us to refer to all weapons together
public abstract class Weapon : MonoBehaviour
{
    // Variables For All Weapons
    [SerializeField] protected float fireRate;

    void Start()
    {
        // Start Attack Sequence
        StartCoroutine(FireLoop());
    }

    private IEnumerator FireLoop()
    {
        // Loop the waiting for attack, then attacking.
        while (true)
        {
            yield return new WaitForSeconds(fireRate);
            Attack(); 
            if (fireRate == 0) break; // FireRate set to 0 means that it is a onetime attack that does not need to be retriggered
        }
    }

    protected abstract void Attack();
}
