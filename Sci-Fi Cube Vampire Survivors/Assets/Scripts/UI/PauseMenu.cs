using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    bool paused = false;
    public GameObject pauseMenu;
    public string MenuScene = "Menu";

    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            paused = togglePause();
        }
    }

    bool togglePause()
	{
		if(Time.timeScale == 0f)
		{
			Time.timeScale = 1f;
            pauseMenu.SetActive(false);
			return(false);
		}
		else
		{
			Time.timeScale = 0f;
            pauseMenu.SetActive(true);
			return(true);
		}
	}

    public void ResumeGame()
    {
        togglePause();
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;  // Unpause the game
        SceneManager.LoadScene(MenuScene);  // Load the Main Menu scene
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;  // Unpause the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reload the current scene
    }
}

