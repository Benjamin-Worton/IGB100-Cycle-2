	using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeMenu : MonoBehaviour
{
    bool paused = false;
    public GameObject upgradeMenu;
    public GameObject player;

    public TMP_Text assaultRifleButtonText;
    public TMP_Text ShoulderButtonText;
    public TMP_Text RoboticButtonText;
    public TMP_Text hoverButtonText;
    public TMP_Text shellButtonText;
    public TMP_Text minesButtonText;
    public TMP_Text beaconButtonText;
    public TMP_Text eaterButtonText;

    private Player playerScript;
    private AssaultRifle assaultRifleScript;
    private ShoulderLasers shoulderScript;
    private RoboticTracks roboticScript;
    private HoverPad hoverScript;
    private PlasmaShell shellScript;
    private EMP1Mines minesScript;
    private RepulseBeacon beaconScript;
    private SteelEaters eaterScript;

    public int assaultRifleCost = 10;
    public int assaultRifleUpgradeCost = 5;
    public int assaultRifleInventorySpace = 2;

    bool hasShoulder = false;// We dont need these variables, please remove the hasWeapon variables
    public int shoulderCost = 10;
    public int shoulderUpgradeCost = 5;
    public int ShoulderInventorySpace = 1;

    bool hasRobotic = false;
    public int roboticCost = 10;
    public int roboticUpgradeCost = 5;
    public int roboticInventorySpace = 1;

    bool hasHover = false;
    public int hoverCost = 10;
    public int hoverUpgradeCost = 5;
    public int hoverInventorySpace = 1;

    bool hasShell = false;
    public int shellCost = 10;
    public int shellUpgradeCost = 5;
    public int shellInventorySpace = 1;

    bool hasMines = false;
    public int minesCost = 10;
    public int minesUpgradeCost = 5;
    public int minesInventorySpace = 1;

    bool hasBeacon = false;
    public int beaconCost = 10;
    public int beaconUpgradeCost = 5;
    public int beaconInventorySpace = 1;

    bool hasEater = false;
    public int eaterCost = 10;
    public int eaterUpgradeCost = 5;
    public int eaterInventorySpace = 1;

    public int spentInventory;

    void Start()
    {
        upgradeMenu.SetActive(false);

        if (player != null)
        {
            playerScript = player.GetComponent<Player>();
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            paused = togglePause();
        }
    }

    bool togglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            upgradeMenu.SetActive(false);
            return false;
        }
        else
        {
            Time.timeScale = 0f;
            upgradeMenu.SetActive(true);
            return true;
        }
    }

    public void EnableScript(string weaponScriptName)
    {
        if (playerScript == null) { return; }
        // Get the type of the weapon script
        System.Type type = System.Type.GetType(weaponScriptName);

        if (type != null) { return; }

        // Check if the weapon script is already attached to the player
        Component existingScript = player.GetComponent(type);

        if (existingScript == null)
        {
            // If the weapon script is not attached, add the script to the player
            player.AddComponent(type);
        }
        else
        {
            // If the script is already attached, just enable it
            MonoBehaviour mono = existingScript as MonoBehaviour;
            if (mono != null)
            {
                mono.enabled = true;
            }
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        upgradeMenu.SetActive(false);
    }

    public void PurchaseAssaultRifleUpgrade()
    {
        if (playerScript == null) { return; }
        if (playerScript.scrap < assaultRifleCost /*&& playerScript.currentInventorySpace < assaultRifleInventorySpace */)
        {
            ChangeButtonText(assaultRifleButtonText, "NOT ENOUGH SCRAP OR SPACE!");
            return;
        }
        // Buy Assault Rifle
        playerScript.scrap -= assaultRifleCost;
        player.AddComponent<AssaultRifle>();

        ChangeButtonText(assaultRifleButtonText, "UPGRADE FOR " + assaultRifleUpgradeCost + " SCRAP");
        EnableScript("AssaultRifleWeapon");
    }

    public void PurchaseShoulderUpgrade()
    {
        if (playerScript == null) { return; }
        if (!hasShoulder && playerScript.scrap >= shoulderCost)
        {
            // Player can afford the assault rifle
            playerScript.scrap -= shoulderCost; // Subtract scrap
            hasShoulder = true;
            player.AddComponent<ShoulderLasers>();

            ChangeButtonText(ShoulderButtonText,"UPGRADE FOR " + shoulderUpgradeCost + " SCRAP");
            EnableScript("ShoulderLasers"); // Enable weapon or upgrade
        }
        else if (hasShoulder && playerScript.scrap >= shoulderUpgradeCost)
        {
            // Player can afford the fire rate upgrade
            playerScript.scrap -= shoulderUpgradeCost; // Subtract scrap

            // Get the AssaultRifleWeapon component
            shoulderScript = player.GetComponent<ShoulderLasers>();

            shoulderScript.UpgradeFireRate();
        }
        else
        {
            // Not enough scrap
            ChangeButtonText(ShoulderButtonText, "NOT ENOUGH SCRAP!");
        }
    }

    public void PurchaseRoboticUpgrade()
    {
        if (playerScript == null) { return; }
        if (!hasRobotic && playerScript.scrap >= roboticCost)
        {
            // Player can afford the assault rifle
            playerScript.scrap -= roboticCost; // Subtract scrap
            hasRobotic = true;
            player.AddComponent<RoboticTracks>();

            ChangeButtonText(RoboticButtonText,"BROUGHT");
            EnableScript("RoboticTracks"); // Enable weapon or upgrade
        }
        else
        {
            // Not enough scrap
            ChangeButtonText(RoboticButtonText, "NOT ENOUGH SCRAP!");
        }
    }

   public void PurchaseHoverUpgrade()
    {
        if (playerScript != null)
        {
            if (!hasHover && playerScript.scrap >= hoverCost)
            {
                playerScript.scrap -= hoverCost;
                hasHover = true;
                ChangeButtonText(hoverButtonText, "PURCHASED");
                EnableScript("HoverPad");
            }
            else
            {
                ChangeButtonText(hoverButtonText, "NOT ENOUGH SCRAP!");
            }
        }
    }

    public void PurchaseShellUpgrade()
    {
        if (playerScript != null)
        {
            if (!hasShell && playerScript.scrap >= shellCost)
            {
                playerScript.scrap -= shellCost;
                hasShell = true;
                ChangeButtonText(shellButtonText, "PURCHASED");
                EnableScript("PlasmaShell");
            }
            else
            {
                ChangeButtonText(shellButtonText, "NOT ENOUGH SCRAP!");
            }
        }
    }

    public void PurchaseMinesUpgrade()
    {
        if (playerScript != null)
        {
            if (!hasMines && playerScript.scrap >= minesCost)
            {
                playerScript.scrap -= minesCost;
                hasMines = true;
                ChangeButtonText(minesButtonText, "PURCHASED");
                EnableScript("EMP1Mines");
            }
            else
            {
                ChangeButtonText(minesButtonText, "NOT ENOUGH SCRAP!");
            }
        }
    }

    public void PurchaseBeaconUpgrade()
    {
        if (playerScript != null)
        {
            if (!hasBeacon && playerScript.scrap >= beaconCost)
            {
                playerScript.scrap -= beaconCost;
                hasBeacon = true;
                ChangeButtonText(beaconButtonText, "PURCHASED");
                EnableScript("RepulseBeacon");
            }
            else
            {
                ChangeButtonText(beaconButtonText, "NOT ENOUGH SCRAP!");
            }
        }
    }

    public void PurchaseEaterUpgrade()
    {
        if (playerScript != null)
        {
            if (!hasEater && playerScript.scrap >= eaterCost)
            {
                playerScript.scrap -= eaterCost;
                hasEater = true;
                ChangeButtonText(eaterButtonText, "PURCHASED");
                EnableScript("SteelEaters");
            }
            else
            {
                ChangeButtonText(eaterButtonText, "NOT ENOUGH SCRAP!");
            }
        }
    }


    public void ChangeButtonText(TMP_Text buttonText, string newText)
    {
        if (buttonText != null)
        {
            buttonText.text = newText;
        }
    }
}