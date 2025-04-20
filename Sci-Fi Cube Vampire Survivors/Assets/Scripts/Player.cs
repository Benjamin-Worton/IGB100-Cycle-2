using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Variables
    [HideInInspector] public Vector3 direction = Vector3.zero;
    [HideInInspector] public bool isDashing = false;

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
            HandleSpeed();
            // Move Main Scene (Player + Camera)
            this.transform.parent.position += (direction.normalized) * speed / 300f;
        }
        

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
}
