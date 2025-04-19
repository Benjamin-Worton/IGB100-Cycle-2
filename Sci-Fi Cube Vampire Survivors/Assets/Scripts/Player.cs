using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Variables
    [SerializeField] private float speed = 0.3f;
    [SerializeField] public float damage = 1f;
    [HideInInspector] public Vector3 direction = Vector3.zero;
    [HideInInspector] public bool isDashing = false;
    

    // References
    [SerializeField] private TrailRenderer bashTrail;

    void Start()
    {
        // Setting Trail Width
        bashTrail.startWidth = 10f;

    }

    void Update()
    {
        // Player Movement
        direction = Vector3.zero;
        // Set Directions
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
            // Move Main Scene (Player + Camera)
            this.transform.parent.position += (direction.normalized) * speed;
        }

        // If Dashing (set by Bash) then turn on trail
        bashTrail.emitting = isDashing;
    }
}
