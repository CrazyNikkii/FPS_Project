using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices.ComTypes;

public class MainMenu : MonoBehaviour
{
    // References
    public AudioMixer audioMixer;
    public Slider masterVolumeS;
    public Slider musicVolumeS;
    public Slider effectsVolumeS;
    public Slider sensSlider;
    public Slider scopedSensSlider;
    public TextMeshProUGUI sensitivityText;
    public TextMeshProUGUI scopedSensitivityText;

    public void Start()
    {
        // Checks if any data saved on player prefs, if not, sets default values. If there is saved data, loads that instead
        if (!PlayerPrefs.HasKey("masterVolume"))
        {
            PlayerPrefs.SetFloat("masterVolume", 1f);
        }
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1f);
        }
        if (!PlayerPrefs.HasKey("effectsVolume"))
        {
            PlayerPrefs.SetFloat("effectsVolume", 1f);
        }
        if (!PlayerPrefs.HasKey("Sensitivity"))
        {
            PlayerPrefs.SetFloat("Sensitivity", 0.1f);
        }
        else
        {
            Load();
        }

        // Moves the sliders to correct positions
        sensSlider.value = PlayerPrefs.GetFloat("Sensitivity");
        scopedSensSlider.value = PlayerPrefs.GetFloat("ScopedSensitivity");
        masterVolumeS.value = PlayerPrefs.GetFloat("masterVolume");
        musicVolumeS.value = PlayerPrefs.GetFloat("musicVolume");
        effectsVolumeS.value = PlayerPrefs.GetFloat("effectsVolume");
    }
    
    public void Update()
    {
        // Shows the sensitivty sliders value to player. Makes it easier to adjust sensitivity.
        sensitivityText.text = (PlayerPrefs.GetFloat("Sensitivity")*1000).ToString("00");
        scopedSensitivityText.text = (PlayerPrefs.GetFloat("ScopedSensitivity") * 1000).ToString("00");
    }

    // Play button
    public void PlayTraining()
    {
        SceneManager.LoadScene("SingleplayerMap");
        Save();
    }

    public void PlayTutorial()
    {
        SceneManager.LoadScene("Tutorial");
        Save();
    }

    // Quit Game button
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting game.");
        Save();
    }

    // Adjusting the sliders sets the value to player prefs
    public void SetMasterVolume(float masterVolume)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10 (masterVolume) * 20);
        PlayerPrefs.SetFloat("masterVolume", masterVolume);
    }
    public void SetMusicVolume(float musicVolume)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(musicVolume) * 20);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
    }
    public void SetEffectsVolume(float effectsVolume)
    {
        audioMixer.SetFloat("effectsVolume", Mathf.Log10(effectsVolume) * 20);
        PlayerPrefs.SetFloat("effectsVolume", effectsVolume);
    }
    
    public void SetSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
    }

    public void SetScopedSensitivity(float scopedSensitivity)
    {
        PlayerPrefs.SetFloat("ScopedSensitivity", scopedSensitivity);
    }

    // Load the data
    public void Load()
    {
        PlayerPrefs.GetFloat("masterVolume");
        PlayerPrefs.GetFloat("musicVolume");
        PlayerPrefs.GetFloat("effectsVolume");
    }

    // Save data
    public void Save()
    {
        PlayerPrefs.Save();
    }
}
