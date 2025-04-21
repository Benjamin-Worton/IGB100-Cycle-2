using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    bool paused = false;
    public GameObject upgradeMenu;
    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        upgradeMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            paused = togglePause();
        }
    }

    bool togglePause()
	{
        if(Time.timeScale == 0f)
		{
			Time.timeScale = 1f;
            upgradeMenu.SetActive(false);
			return(false);
		}
		else
		{
			Time.timeScale = 0f;
            upgradeMenu.SetActive(true);
			return(true);	
		}
    }

    public void EnableScript(string scriptName)
    {
        if (player != null)
        {
            System.Type type = System.Type.GetType(scriptName);

            Component weaponScript = player.GetComponent(type);
            if (weaponScript != null)
            {
                MonoBehaviour mono = weaponScript as MonoBehaviour;
                if (mono != null)
                {
                    mono.enabled = true;
                }

            }
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;  // Unpause the game
        upgradeMenu.SetActive(false);  // Hide the pause menu
    }
}
