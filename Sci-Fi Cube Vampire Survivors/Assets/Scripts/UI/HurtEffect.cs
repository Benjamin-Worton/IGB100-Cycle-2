using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEffect : MonoBehaviour
{
    public GameObject[] uiDisplays; 
    public float flashDuration = 0.2f;

    public IEnumerator HurtFlash()
    {
        // Create a list from the array to shuffle
        List<GameObject> shuffledDisplays = new List<GameObject>(uiDisplays);

        // Shuffle the list
        for (int i = 0; i < shuffledDisplays.Count; i++)
        {
            GameObject temp = shuffledDisplays[i];
            int randomIndex = Random.Range(i, shuffledDisplays.Count);
            shuffledDisplays[i] = shuffledDisplays[randomIndex];
            shuffledDisplays[randomIndex] = temp;
        }

        // Flash each UI display in random order
        foreach (GameObject display in shuffledDisplays)
        {
            if (display == null) continue;

            // Disable only if not already disabled (defensive)
            if (!display.activeSelf)
            {
                display.SetActive(true);
            }
            yield return new WaitForSeconds(flashDuration);

            if (display.activeSelf)
            {
                display.SetActive(false);
            }
        }
    }
}
