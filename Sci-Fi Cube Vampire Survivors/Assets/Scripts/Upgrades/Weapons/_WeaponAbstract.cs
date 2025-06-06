using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// A basic abstract class that is given to all weapons to save some time and allow us to refer to all weapons together
public abstract class WeaponAbstract : UpgradeAbstract
{
    // Variables For All Weapons
    [Header("Stats")]
    protected float fireRate;
    [HideInInspector] public GameObject weapon;

    protected virtual void Start()
    {
        // Start Attack Sequence
        StartCoroutine(FireLoop());
    }


    /// <summary>
    /// Calls the <see cref="Attack"/> function each time fireRate * cooldownMultiplier is elapsed
    /// </summary>
    /// <returns></returns>
    protected IEnumerator FireLoop()
    {
        // Loop the waiting for attack, then attacking.
        while (true)
        {
            yield return new WaitForSeconds(fireRate * gameObject.GetComponent<Player>().CooldownMultiplier);
            Attack(); 
            if (fireRate == 0) break; // FireRate set to 0 means that it is a onetime attack that does not need to be retriggered
        }
    }

    public virtual void SetOrbitPosition(Vector3 pos)
    {
        if (weapon != null)
        {
            weapon.transform.position = pos;
        }
    }

    /// <summary>
    /// Performs the weapon's primary attack.
    /// </summary>
    /// <remarks>
    /// Is Called by <see cref="FireLoop"/> function.
    /// </remarks>
    protected abstract void Attack();
}
