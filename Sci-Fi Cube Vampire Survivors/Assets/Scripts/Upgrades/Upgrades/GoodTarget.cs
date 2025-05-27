using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodTarget : MonoBehaviour
{
    private GameObject GoodtargetPrefab;
    private GameObject GoodtargetObject;
    [SerializeField] private float DistanceFromEnemy = 1f;
    [SerializeField] private float floatAmplitude = 0.2f; // How high/low it moves
    [SerializeField] private float floatFrequency = 2f;   // How fast it moves

    private float initialYOffset;

    void Awake()
    {
        // Set up target prefab
        GoodtargetPrefab = Resources.Load<GameObject>("Prefabs/greenArrowPrefab");

        // Create point above enemies head
        GoodtargetObject = Instantiate(GoodtargetPrefab, transform.position + Vector3.up * DistanceFromEnemy, Quaternion.identity);

        initialYOffset = DistanceFromEnemy;
    }

    void Update()
    {
        // Calculate vertical bobbing offset
        float bobbingOffset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;

        // Update position with bobbing
        GoodtargetObject.transform.position = transform.position + Vector3.up * (initialYOffset + bobbingOffset);
    }

    void OnDestroy()
    {
        if (GoodtargetObject != null)
        {
            Destroy(GoodtargetObject);
        }
    }
}
