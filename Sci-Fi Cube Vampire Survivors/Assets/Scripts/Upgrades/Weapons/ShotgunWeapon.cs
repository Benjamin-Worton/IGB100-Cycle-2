using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Shotgun : WeaponAbstract
{
    private GameObject bulletPrefab;
    private GameObject ShotgunPrefab;
    private GameObject ShotgunObject;

    [SerializeField] private float damage = 30f;
    [SerializeField] private float range = 3.5f;
    [SerializeField] private int numOfBullets = 5;
    [SerializeField] private float spreadAngle = 60f;
    [SerializeField] private float DistanceFromPlayer = 2f;





    private void Awake()
    {
        fireRate = 1f;  // Default fire rate

        // Set up weapon prefabs
        ShotgunPrefab = Resources.Load<GameObject>("Prefabs/Shotgun");
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");

        // Create Shotgun Object above player's head
        ShotgunObject = Instantiate(ShotgunPrefab, transform.position + Vector3.right * DistanceFromPlayer, Quaternion.identity);
    }

    void Update()
    {
        // Keep shotgun locked to player
        ShotgunObject.transform.position = transform.position + Vector3.right * DistanceFromPlayer;


    }

    public override void Remove()
    {
        Destroy(ShotgunObject);
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
        ShotgunObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Flip Shotgun if necessary
        Vector3 scale = ShotgunObject.transform.localScale;
        scale.y = direction.x < 0 ? -Mathf.Abs(scale.y) : Mathf.Abs(scale.y);
        ShotgunObject.transform.localScale = scale;

        // Instantiate the bullet and it's stats as well as give them an offset angle for shotgun spread
        for (float bulletNum = 1f; bulletNum <= numOfBullets; bulletNum++)
        {
            float offSetAngle = ((bulletNum / (float)numOfBullets) - 0.5f) * spreadAngle;
            GameObject Bullet = Instantiate(bulletPrefab, ShotgunObject.transform.GetChild(0).transform.position, ShotgunObject.transform.rotation * Quaternion.Euler(0, 0, offSetAngle));
            Bullet bulletScript = Bullet.GetComponent<Bullet>();
            bulletScript.damage = damage;
            bulletScript.destroyOnCollision = false;
            bulletScript.range = range;
        }

    }
    #endregion
}
