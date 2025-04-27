using UnityEngine;
using TMPro; // Make sure to use TextMeshPro for better UI text rendering

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; // Singleton pattern for easy access
    public int score = 0; // Player's score
    public TextMeshProUGUI scoreText; // Reference to the UI text component

    void Awake()
    {
        // Ensure there's only one instance of ScoreManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // This keeps the object from being destroyed when loading a new scene
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance if already exists
        }
    }

    void Start()
    {
        UpdateScoreText();
    }

    // Method to add points
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    // Update the UI text
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }
}