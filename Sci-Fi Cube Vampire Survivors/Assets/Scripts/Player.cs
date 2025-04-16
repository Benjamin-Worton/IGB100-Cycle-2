using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Variables
    private float speed = 0.3f;
    [HideInInspector] public Vector3 direction = Vector3.zero;
    [HideInInspector] public bool isDashing = false;

    // References
    [SerializeField] private TrailRenderer bashTrail;

    // Start is called before the first frame update
    void Start()
    {
        bashTrail.startWidth = 10f;

    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        direction = Vector3.zero;
        if (Input.GetKey(KeyCode.S))
        {
            direction.y -= 1f;
        }
        if (Input.GetKey(KeyCode.W))
        {
            direction.y += 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction.x -= 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction.x += 1f;
        }
        if (direction.magnitude != 0f)
        {
            this.transform.parent.transform.position += (direction / direction.magnitude) * speed;
        }
        bashTrail.emitting = isDashing;
    }
}
