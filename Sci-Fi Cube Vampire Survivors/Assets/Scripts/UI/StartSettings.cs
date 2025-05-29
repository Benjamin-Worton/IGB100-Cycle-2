using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSettings : MonoBehaviour
{
    public GameObject startSettings;
    public Slider volumeSlider;
    public Toggle tutorialToggle;
    public string MenuScene = "Main Menu";

    private const string tutorialPrefKey = "ShowTutorial";

    void Start()
    {
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(VolumeControl);
            VolumeControl(volumeSlider.value); // Set initial volume from slider
        }

        if (tutorialToggle != null)
        {
            bool showTutorial = PlayerPrefs.GetInt(tutorialPrefKey, 1) == 1;
            tutorialToggle.isOn = showTutorial;

            tutorialToggle.onValueChanged.AddListener(delegate { TutorialControl(); });
        }
    }

    public void Return()
    {
        if (AudioManager.Instance != null) { AudioManager.Instance.PlaySFX("buttonpress"); }
        SceneManager.LoadScene(MenuScene);
    }

    public void VolumeControl(float value)
    {
        AudioManager.Instance.musicSource.volume = value / 100f;
        AudioManager.Instance.sfxSource.volume = value / 100f;
    }

    public void TutorialControl()
    {
        if (tutorialToggle != null)
        {
            bool skipTutorial = tutorialToggle.isOn;
            PlayerPrefs.SetInt(tutorialPrefKey, skipTutorial ? 0 : 1);
            PlayerPrefs.Save();
        }
    }

    public void QuitGame()
    {
        if (AudioManager.Instance != null) { AudioManager.Instance.PlaySFX("buttonpress"); }
        Application.Quit();
    }

    public static bool IsTutorialEnabled()
    {
        // Tutorial is enabled when it's NOT skipped
        return PlayerPrefs.GetInt(tutorialPrefKey, 1) == 1;
    }
}