using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Shotgun : WeaponAbstract, IOrbiting
{
    private GameObject bulletPrefab;
    private GameObject ShotgunPrefab;

    [SerializeField] private float damage = 30f;
    [SerializeField] private float range = 3.5f;
    [SerializeField] private int numOfBullets = 5;
    [SerializeField] private float spreadAngle = 60f;





    protected override void Start()
    {
        fireRate = 1f;  // Default fire rate

        // Set up weapon prefabs
        ShotgunPrefab = Resources.Load<GameObject>("Prefabs/Shotgun");
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Shotgun Pellet");

        // Create Shotgun Object above player's head
        weapon = Instantiate(ShotgunPrefab, transform.position, Quaternion.identity);
        GetComponent<WeaponManager>().AddWeapon(this);

        base.Start();
    }

    public override void Remove()
    {
        GetComponent<WeaponManager>().RemoveWeapon(this);
        Destroy(weapon);
        Destroy(this);
    }

    

    #region Attack Functions
    // Fires in player direction
    protected override void Attack()
    {
        Shoot();
    }

    private void Shoot()
    {
        // Set direction and rotation for the shotgun
        Vector3 direction = gameObject.GetComponent<Player>().lastKnownDirection;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        weapon.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Flip Shotgun if necessary
        Vector3 scale = weapon.transform.localScale;
        scale.y = direction.x < 0 ? -Mathf.Abs(scale.y) : Mathf.Abs(scale.y);
        weapon.transform.localScale = scale;

        // Instantiate the bullet and it's stats as well as give them an offset angle for shotgun spread
        for (float bulletNum = 1f; bulletNum <= numOfBullets; bulletNum++)
        {
            float offSetAngle = ((bulletNum / (float)numOfBullets) - 0.5f) * spreadAngle;
            GameObject Bullet = Instantiate(bulletPrefab, weapon.transform.GetChild(0).transform.position, weapon.transform.rotation * Quaternion.Euler(0, 0, offSetAngle));
            Bullet bulletScript = Bullet.GetComponent<Bullet>();
            bulletScript.damage = damage;
            bulletScript.destroyOnCollision = true;
            bulletScript.range = range;
        }

    }
    #endregion
}
