using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private GameObject targetPrefab;
    private GameObject targetObject;
    [SerializeField] private float DistanceFromEnemy = 1f;
    [SerializeField] private float floatAmplitude = 0.2f; // How high/low it moves
    [SerializeField] private float floatFrequency = 2f;   // How fast it moves

    private float initialYOffset;

    void Awake()
    {
        // Set up target prefab
        targetPrefab = Resources.Load<GameObject>("Prefabs/redArrowPrefab");

        // Create point above enemies head
        targetObject = Instantiate(targetPrefab, transform.position + Vector3.up * DistanceFromEnemy, Quaternion.identity);

        initialYOffset = DistanceFromEnemy;
    }

    void Update()
    {
        // Calculate vertical bobbing offset
        float bobbingOffset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;

        // Update position with bobbing
        targetObject.transform.position = transform.position + Vector3.up * (initialYOffset + bobbingOffset);
    }

    void OnDestroy()
    {
        if (targetObject != null)
        {
            Destroy(targetObject);
        }
    }
}
