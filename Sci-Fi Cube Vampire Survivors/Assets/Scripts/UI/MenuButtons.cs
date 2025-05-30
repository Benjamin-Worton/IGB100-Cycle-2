using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public string ControlScene = "Controls";
    public string GamePlayScene = "Gameplay";

    private void Start()
    {
        StartCoroutine(PlayMenuMusicDelayed());
    }

    private IEnumerator PlayMenuMusicDelayed()
    {
        yield return new WaitForEndOfFrame();

        if (SceneManager.GetActiveScene().name == "Main Menu" && !AudioManager.Instance.musicSource.isPlaying) { AudioManager.Instance.PlayMusic("menumusic"); }

    }
    public void SettingsScene()
    {
        if (AudioManager.Instance != null) { AudioManager.Instance.PlaySFX("buttonpress"); }
        SceneManager.LoadScene(ControlScene);
    }

    public void GameScene()
    {
        if (AudioManager.Instance != null) { AudioManager.Instance.PlaySFX("buttonpress"); }
        SceneManager.LoadScene(GamePlayScene);
    }

    public void QuitApplication()
    {
        if (AudioManager.Instance != null) { AudioManager.Instance.PlaySFX("buttonpress"); }
        
        Application.Quit();
    }
}
