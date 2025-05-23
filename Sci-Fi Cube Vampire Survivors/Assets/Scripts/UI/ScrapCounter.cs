using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEngine.GraphicsBuffer;

public class ScrapCounter : MonoBehaviour
{
    public TextMeshProUGUI scrapText; // Reference to the UI text component
    private Player playerScript;

    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        UpdateScrapText();
    }

    void Update()
    {
        UpdateScrapText();
    }

    public void AddScrap(int amount)
    {
        playerScript.scrap += amount;
        UpdateScrapText();
    }

    public int GetTotalScrap()
    {
        return playerScript.scrap;
    }

    public void SetScrap(int amount)
    {
        playerScript.scrap = amount;
        UpdateScrapText();
    }

    private void UpdateScrapText()
    {
        if (scrapText != null)
        {
            scrapText.text = "Scrap: " + playerScript.scrap.ToString();
        }
    }
}