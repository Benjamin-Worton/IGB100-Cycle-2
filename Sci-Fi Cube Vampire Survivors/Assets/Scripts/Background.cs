using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Camera camera;
    private float width;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        camera = Camera.main;
        width = spriteRenderer.bounds.size.x; // Get the width of the background image
    }

    void Update()
    {
        // Calculate the camera's horizontal position
        float cameraPositionX = camera.transform.position.x;

        // Move the background to the left
        if (cameraPositionX > transform.position.x + width / 2)
        {
            transform.position = new Vector3(transform.position.x + width, transform.position.y, transform.position.z);
        }

        // If background goes out of screen, move it to the other side to repeat
        if (cameraPositionX < transform.position.x - width / 2)
        {
            transform.position = new Vector3(transform.position.x - width, transform.position.y, transform.position.z);
        }
    }
}