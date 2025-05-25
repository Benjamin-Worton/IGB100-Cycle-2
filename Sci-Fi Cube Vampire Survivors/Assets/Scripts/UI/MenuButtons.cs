using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            if (SceneManager.GetActiveScene().name == "Main Menu" && AudioManager.Instance != null) { AudioManager.Instance.PlayMusic("menumusic"); }
        }
    }
    public void ChangeScene(string sceneName)
    {
        if (AudioManager.Instance != null) { AudioManager.Instance.PlaySFX("buttonpress"); }
        SceneManager.LoadScene(sceneName);
    }

    public void QuitApplication()
    {
        if (AudioManager.Instance != null) { AudioManager.Instance.PlaySFX("buttonpress"); }
        
        Application.Quit();
    }
}
