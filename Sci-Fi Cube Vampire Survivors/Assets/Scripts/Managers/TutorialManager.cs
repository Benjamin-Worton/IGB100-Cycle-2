using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject blackScreen;
    public GameObject[] tutorialDisplays;
    public GameObject shopDisplay;

    public GameObject[] hudUi;

    public float animationDuration = 0.5f;
    public Vector3 startScale = new Vector3(0.5f, 0.5f, 0.5f);
    public Vector3 endScale = Vector3.one;

    public Vector3 startOffset = new Vector3(0, -800f, 0);
    public Vector3 exitOffset = new Vector3(0, 800f, 0);

    private Vector3 centerPosition;

    private void Start()
    {
        centerPosition = Vector3.zero;
        foreach (GameObject uI in hudUi)
        {
            uI.SetActive(false);
        }
    }

    public IEnumerator TutorialTip()
    {
        Time.timeScale = 0;
        Debug.Log(Time.timeScale);
        blackScreen.SetActive(true);

        foreach (GameObject display in tutorialDisplays)
        {
            display.SetActive(true);
            yield return StartCoroutine(AnimatePopup(display));

            yield return StartCoroutine(WaitForInput());

            yield return StartCoroutine(AnimateExit(display));
            display.SetActive(false);
        }

        yield return StartCoroutine(FadeOutBlackScreen(0.5f));
        Time.timeScale = 1;
        foreach (GameObject uI in hudUi)
        {
            uI.SetActive(true);
        }
    }

    public void NoIntro()
    {
        foreach (GameObject uI in hudUi)
        {
            uI.SetActive(true);
        }
    }

    public IEnumerator ShopTip()
    {
        shopDisplay.SetActive(true);
        yield return StartCoroutine(AnimatePopup(shopDisplay));

        yield return StartCoroutine(WaitForInput());

        yield return StartCoroutine(AnimateExit(shopDisplay));
        shopDisplay.SetActive(false);
    }

    private IEnumerator AnimatePopup(GameObject display)
    {
        RectTransform rect = display.GetComponent<RectTransform>();
        Vector3 targetPosition = centerPosition;
        Vector3 startPosition = targetPosition + startOffset;

        rect.localPosition = startPosition;
        rect.localScale = startScale;

        float elapsed = 0f;
        while (elapsed < animationDuration)
        {
            float t = elapsed / animationDuration;
            rect.localPosition = Vector3.Lerp(startPosition, targetPosition, EaseOutBack(t));
            rect.localScale = Vector3.Lerp(startScale, endScale, EaseOutBack(t));

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        rect.localPosition = targetPosition;
        rect.localScale = endScale;
    }

    private IEnumerator AnimateExit(GameObject display)
    {
        RectTransform rect = display.GetComponent<RectTransform>();
        Vector3 startPosition = centerPosition;
        Vector3 endPosition = centerPosition + exitOffset;

        float elapsed = 0f;
        while (elapsed < animationDuration)
        {
            float t = elapsed / animationDuration;
            rect.localPosition = Vector3.Lerp(startPosition, endPosition, EaseInBack(t));
            rect.localScale = Vector3.Lerp(endScale, startScale, EaseInBack(t));

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        rect.localPosition = endPosition;
        rect.localScale = startScale;
    }

    private IEnumerator FadeOutBlackScreen(float duration)
    {
        Image img = blackScreen.GetComponent<Image>();
        Color originalColor = img.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            img.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1f, 0f, t));
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        img.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        blackScreen.SetActive(false);
    }

    private IEnumerator WaitForInput()
    {
        while (!Input.anyKeyDown && !Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
    }

    // Easing functions
    private float EaseOutBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1;
        return 1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2);
    }

    private float EaseInBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1;
        return c3 * t * t * t - c1 * t * t;
    }
}