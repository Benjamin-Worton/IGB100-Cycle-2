using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] tutorialDisplays; 
    public GameObject shopDisplay;
    [SerializeField] private float tipDuraction = 3f;
    public IEnumerator TutorialTip()
    {
        Time.timeScale = 0;

        foreach (GameObject display in tutorialDisplays)
        {
            display.SetActive(true);
            yield return new WaitForSecondsRealtime(tipDuraction);
            display.SetActive(false);
        }

        Time.timeScale = 1;
    }

    public IEnumerator ShopTip()
    {
        shopDisplay.SetActive(true);
        yield return new WaitForSeconds(tipDuraction);
        shopDisplay.SetActive(false);
    }
}
