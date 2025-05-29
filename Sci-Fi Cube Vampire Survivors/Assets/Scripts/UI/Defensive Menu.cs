using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveMenu : MonoBehaviour
{
    public GameObject upgradeMenuStart;
    public GameObject player;
    public HoverMessageController hoverMessageController;

    private int currentUpgradeCost;

    public void BackToMainMenu()
    {
        if (upgradeMenuStart != null)
        {
            upgradeMenuStart.SetActive(true);
        }

        gameObject.SetActive(false);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            BackToMainMenu();
        }
    }

    public void UpgradePrice(int cost)
    {
        currentUpgradeCost = cost;
    }

    public void EnableScript(string weaponScriptName)
    {
        if (player == null)
        {
            Debug.LogWarning("Player GameObject not assigned.");
            return;
        }

        Player playerScript = player.GetComponent<Player>();
        if (playerScript == null)
        {
            Debug.LogError("Player script not found on player object.");
            return;
        }

        if (playerScript.scrap < currentUpgradeCost)
        {
            hoverMessageController.ShowMessage("Not Enough Scrap!");
            Debug.Log("Not enough scrap to buy this upgrade.");
            return;
        }

        playerScript.scrap -= currentUpgradeCost;
        hoverMessageController.ShowMessage("Upgrade Bought!");
        Debug.Log($"Upgrade bought! Remaining scrap: {playerScript.scrap}");

        System.Type type = System.Type.GetType(weaponScriptName);
        if (type == null)
        {
            Debug.LogError($"Script type '{weaponScriptName}' not found.");
            return;
        }

        player.AddComponent(type);
    }
}