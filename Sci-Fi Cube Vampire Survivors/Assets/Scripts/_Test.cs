using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Enemy collided with " + collision.gameObject.name);
    }
}