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
            PlayerPrefs.SetFloat("Sensitivity", 1f);
        }
        else
        {
            Load();
        }
        sensSlider.value = PlayerPrefs.GetFloat("Sensitivity");
        scopedSensSlider.value = PlayerPrefs.GetFloat("ScopedSensitivity");
        masterVolumeS.value = PlayerPrefs.GetFloat("masterVolume");
        musicVolumeS.value = PlayerPrefs.GetFloat("musicVolume");
        effectsVolumeS.value = PlayerPrefs.GetFloat("effectsVolume");
    }
    
    public void Update()
    {
        sensitivityText.text = (PlayerPrefs.GetFloat("Sensitivity")*1000).ToString("00");
        scopedSensitivityText.text = (PlayerPrefs.GetFloat("ScopedSensitivity") * 1000).ToString("00");
    }
    public void PlayTraining()
    {
        SceneManager.LoadScene("SingleplayerMap");
        Save();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting game.");
        Save();
    }

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

    public void Load()
    {
        PlayerPrefs.GetFloat("masterVolume");
        PlayerPrefs.GetFloat("musicVolume");
        PlayerPrefs.GetFloat("effectsVolume");
    }

    public void Save()
    {
        PlayerPrefs.Save();
    }
}
