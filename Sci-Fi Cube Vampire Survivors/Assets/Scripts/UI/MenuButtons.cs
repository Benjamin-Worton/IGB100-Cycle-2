using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(PlayMenuMusicDelayed());
    }

    private IEnumerator PlayMenuMusicDelayed()
    {
        yield return new WaitForEndOfFrame();

        if (SceneManager.GetActiveScene().name == "Main Menu" && !AudioManager.Instance.musicSource.isPlaying) { AudioManager.Instance.PlayMusic("menumusic"); }

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
