using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IonLaserObject : MonoBehaviour
{
    [HideInInspector] public float damage = 10f;
    [HideInInspector] public float lifetime = 1f;

    private void Start()
    {
        StartCoroutine(RemoveAfterSeconds(lifetime));
    }

    private IEnumerator RemoveAfterSeconds(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
