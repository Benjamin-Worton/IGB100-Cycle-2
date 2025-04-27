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
    private Player playerScript;
    private AssaultRifleWeapon assaultRifleScript;
    private ShoulderLasers shoulderScript;
    private RoboticTracks roboticScript; 

    bool hasAssaultRifle = false;
    public int assaultRifleCost = 10;
    public int assaultRifleUpgradeCost = 5;

    bool hasShoulder = false;
    public int shoulderCost = 10;
    public int shoulderUpgradeCost = 5;

    bool hasRobotic = false;
    public int roboticCost = 10;
    public int roboticUpgradeCost = 5;

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
        if (player != null)
        {
            // Get the type of the weapon script
            System.Type type = System.Type.GetType(weaponScriptName);

            if (type != null)
            {
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
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        upgradeMenu.SetActive(false);
    }

    public void PurchaseAssaultRifleUpgrade()
    {
        if (playerScript != null)
        {
            if (!hasAssaultRifle && playerScript.scrap >= assaultRifleCost)
            {
                // Player can afford the assault rifle
                playerScript.scrap -= assaultRifleCost; // Subtract scrap
                hasAssaultRifle = true;
                ChangeButtonText(assaultRifleButtonText, "UPGRADE FOR " + assaultRifleUpgradeCost + " SCRAP");
                EnableScript("AssaultRifleWeapon"); // Enable weapon or upgrade
            }
            else if (hasAssaultRifle && playerScript.scrap >= assaultRifleUpgradeCost)
            {
                // Player can afford the fire rate upgrade
                playerScript.scrap -= assaultRifleUpgradeCost; // Subtract scrap

                // Get the AssaultRifleWeapon component
                assaultRifleScript = player.GetComponent<AssaultRifleWeapon>();

                assaultRifleScript.UpgradeFireRate();  // Upgrade fire rate
            }
            else
            {
                // Not enough scrap
                ChangeButtonText(assaultRifleButtonText, "NOT ENOUGH SCRAP!");
            }

            // Start the coroutine to revert the button text after 0.1 seconds
            StartCoroutine(RevertButtonText(0.1f));
        }
    }

    public void PurchaseShoulderUpgrade()
    {
        if (playerScript != null)
        {
            if (!hasShoulder && playerScript.scrap >= shoulderCost)
            {
                // Player can afford the assault rifle
                playerScript.scrap -= shoulderCost; // Subtract scrap
                hasShoulder = true;
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

            // Start the coroutine to revert the button text after 0.1 seconds
            StartCoroutine(RevertButtonText(0.1f));
        }
    }

    public void PurchaseRoboticUpgrade()
    {
        if (playerScript != null)
        {
            if (!hasRobotic && playerScript.scrap >= roboticCost)
            {
                // Player can afford the assault rifle
                playerScript.scrap -= roboticCost; // Subtract scrap
                hasRobotic = true;
                ChangeButtonText(RoboticButtonText,"BROUGHT");
                EnableScript("RoboticTracks"); // Enable weapon or upgrade
            }
            else
            {
                // Not enough scrap
                ChangeButtonText(RoboticButtonText, "NOT ENOUGH SCRAP!");
            }

            // Start the coroutine to revert the button text after 0.1 seconds
            StartCoroutine(RevertButtonText(0.1f));
        }
    }

    public void ChangeButtonText(TMP_Text buttonText, string newText)
    {
        if (buttonText != null)
        {
            buttonText.text = newText;
        }
    }

    // Coroutine to revert the button text after a delay
    IEnumerator RevertButtonText(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified time
    }
}