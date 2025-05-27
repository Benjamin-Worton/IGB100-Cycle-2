using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveMenu : MonoBehaviour
{
    public GameObject upgradeMenuStart;
    public GameObject player;
    public HoverMessageController hoverMessageController;

    public int upgradeCost = 10;

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

        // Scrap check
        if (playerScript.scrap < upgradeCost)
        {
            hoverMessageController.ShowMessage("Not Enough Scrap!");
            Debug.Log("Not enough scrap to buy this upgrade.");
            return;
        }

        // Deduct scrap and add to inventory
        playerScript.scrap -= upgradeCost;

        hoverMessageController.ShowMessage("Upgrade Bought!");
        Debug.Log($"Upgrade bought! Remaining scrap: {playerScript.scrap}");

        // Add or enable the script
        System.Type type = System.Type.GetType(weaponScriptName);
        if (type == null)
        {
            Debug.LogError($"Script type '{weaponScriptName}' not found.");
            return;
        }

        Component existingScript = player.GetComponent(type);

        if (existingScript == null)
        {
            player.AddComponent(type);
            Debug.Log($"{weaponScriptName} added to player.");
        }
        else
        {
            MonoBehaviour mono = existingScript as MonoBehaviour;
            if (mono != null && !mono.enabled)
            {
                mono.enabled = true;
                Debug.Log($"{weaponScriptName} enabled on player.");
            }
        }
    }
}