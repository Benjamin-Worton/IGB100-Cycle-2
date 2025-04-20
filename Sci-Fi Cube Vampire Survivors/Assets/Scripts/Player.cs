using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Variables
    [HideInInspector] public Vector2 direction = Vector2.zero;
    [HideInInspector] public bool isDashing = false;
    private Rigidbody2D rb;

    // Stats
    public float health = 100f;
    public float armour = 0f;
    public float speed = 1f;



    // References
    [SerializeField] private TrailRenderer bashTrail;

    void Start()
    {
        // Setting Trail Width
        bashTrail.startWidth = 10/150f;
        rb = GetComponent<Rigidbody2D>();

    }

    private void FixedUpdate()
    {
        if (direction.magnitude != 0f)
        {
            HandleSpeed();
            // Move Main Scene (Player + Camera)
            rb.MovePosition(rb.position + direction.normalized * speed * Time.fixedDeltaTime);
        }
    }

    void Update()
    {
        // Player Movement
        direction = Vector2.zero;
        // Set Directions
        if (Input.GetKey(KeyCode.W)) direction.y += 1f;
        if (Input.GetKey(KeyCode.S)) direction.y -= 1f;
        if (Input.GetKey(KeyCode.A)) direction.x -= 1f;
        if (Input.GetKey(KeyCode.D)) direction.x += 1f;

        // If Dashing (set by Bash) then turn on trail
        bashTrail.emitting = isDashing;
    }
    private void HandleSpeed()
    {
        speed = 1f;

        if (gameObject.GetComponent<RoboticTracks>() != null)
        {
            speed = gameObject.GetComponent<RoboticTracks>().HandleSpeed(speed);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
