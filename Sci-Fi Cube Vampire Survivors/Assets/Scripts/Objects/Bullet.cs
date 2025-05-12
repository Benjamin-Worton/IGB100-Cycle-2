using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;
    public bool destroyOnCollision = false;
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * Time.deltaTime * 10f;
        if (Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) > 50f)
        {
            Destroy(gameObject);
        }
    }
}
