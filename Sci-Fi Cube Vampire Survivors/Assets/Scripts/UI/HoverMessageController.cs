using UnityEngine;
using TMPro;

public class HoverMessageController : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public float fadeDuration = 1.5f;

    private float timer;
    private bool isShowing;
    private Vector3 startPosition;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup.alpha = 0f;
    }

    void Update()
    {
        if (isShowing)
        {
            // Float up from the start position
            float progress = 1f - (timer / fadeDuration);
            float floatUpAmount = progress * 30f; // How far it floats up
            rectTransform.position = startPosition + new Vector3(0, floatUpAmount, 0);

            // Fade out using unscaled time
            timer -= Time.unscaledDeltaTime;
            canvasGroup.alpha = timer / fadeDuration;

            if (timer <= 0f)
            {
                isShowing = false;
                canvasGroup.alpha = 0f;
            }
        }
    }

    public void ShowMessage(string message)
    {
        messageText.text = message;
        startPosition = Input.mousePosition; // Record once
        rectTransform.position = startPosition;
        canvasGroup.alpha = 1f;
        timer = fadeDuration;
        isShowing = true;
    }
}