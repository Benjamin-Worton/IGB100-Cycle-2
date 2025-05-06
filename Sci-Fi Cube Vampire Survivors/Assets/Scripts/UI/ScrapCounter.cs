using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScrapCounter : MonoBehaviour
{
    public static ScrapCounter instance; // Singleton pattern for easy access
    public TextMeshProUGUI scrapText; // Reference to the UI text component
    private int totalScrap = 0; // Track total scrap

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    void Start()
    {
        UpdateScrapText();
    }

    // Method to add scrap
    public void AddScrap(int amount)
    {
        totalScrap += amount;
        UpdateScrapText();
    }

    // Update the UI text
    private void UpdateScrapText()
    {
        if (scrapText != null)
        {
            scrapText.text = "Scrap: " + totalScrap.ToString();
        }
    }

    // Optional: Accessor for total scrap
    public int GetTotalScrap()
    {
        return totalScrap;
    }

    public void SetScrap(int amount)
    {
        totalScrap = amount;
        UpdateScrapText();
    }
    }