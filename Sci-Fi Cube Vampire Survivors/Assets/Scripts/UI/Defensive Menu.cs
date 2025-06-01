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
            return;
        }

        Player playerScript = player.GetComponent<Player>();
        if (playerScript == null)
        {
            return;
        }

        if (playerScript.scrap < currentUpgradeCost)
        {
            hoverMessageController.ShowMessage("Not Enough Scrap!");
            return;
        }

        playerScript.scrap -= currentUpgradeCost;
        hoverMessageController.ShowMessage("Upgrade Bought!");

        System.Type type = System.Type.GetType(weaponScriptName);
        if (type == null)
        {
            return;
        }

        player.AddComponent(type);
    }
}