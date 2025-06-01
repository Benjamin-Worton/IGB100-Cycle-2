using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenuStart : MonoBehaviour
{
    public GameObject offensiveMenu;
    public GameObject defensiveMenu;
    public GameObject buffs_MobilityMenu;
    public GameObject upgradeMenuStart; // Assigned in Inspector

    private bool paused = false;

    void Start()
    {
        Time.timeScale = 1f;

        if (upgradeMenuStart != null)
        {
            upgradeMenuStart.SetActive(false); // Hide at start
        }

        offensiveMenu?.SetActive(false);
        defensiveMenu?.SetActive(false);
        buffs_MobilityMenu?.SetActive(false);
    }

    public void OnLevelUp()
    {
        paused = TogglePause();
    }

    bool TogglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;

            if (upgradeMenuStart != null)
                upgradeMenuStart.SetActive(false);

            return false;
        }
        else
        {
            Time.timeScale = 0f;

            if (upgradeMenuStart != null)
                upgradeMenuStart.SetActive(true);

            return true;
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;

        if (upgradeMenuStart != null)
            upgradeMenuStart.SetActive(false);

        paused = false;
    }

    public void OpenMenu(GameObject menuPrefab)
    {
        offensiveMenu?.SetActive(false);
        defensiveMenu?.SetActive(false);
        buffs_MobilityMenu?.SetActive(false);

        if (menuPrefab != null)
        {
            menuPrefab.SetActive(true);
        }
    }
}