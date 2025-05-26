using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] tutorialDisplays; 
    public GameObject shopDisplay;
    public float tipDuraction = 1f;

    public IEnumerator TutorialTip()
    {
        foreach (GameObject display in tutorialDisplays)
        {
            display.SetActive(true);
            yield return new WaitForSeconds(tipDuraction);
            display.SetActive(false);
        }
    }

    public IEnumerator ShopTip()
    {
        shopDisplay.SetActive(true);
        yield return new WaitForSeconds(tipDuraction);
        shopDisplay.SetActive(false);
    }
}
