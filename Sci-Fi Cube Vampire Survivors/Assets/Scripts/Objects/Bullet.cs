using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public float damage = 10f;
    [HideInInspector] public bool destroyOnCollision = false;
    [HideInInspector] public float range = 30f;
    [HideInInspector] public bool isMerciless = false;
    [HideInInspector] public bool isBurning = false;
    [HideInInspector] public float stunDuration = 0f;



    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * Time.deltaTime * 10f;
        if (Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) > range)
        {
            Destroy(gameObject);
        }
    }
}
