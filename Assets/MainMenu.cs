using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider masterVolumeS;
    public Slider musicVolumeS;
    public Slider effectsVolumeS;

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
        else
        {
            Load();
        }

        masterVolumeS.value = PlayerPrefs.GetFloat("masterVolume");
        musicVolumeS.value = PlayerPrefs.GetFloat("musicVolume");
        effectsVolumeS.value = PlayerPrefs.GetFloat("effectsVolume");
    }
    public void PlayTraining()
    {
        SceneManager.LoadScene("SingleplayerMap");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting game.");
        Save();
    }

    public void SetMasterVolume(float masterVolume)
    {
        audioMixer.SetFloat("masterVolume", masterVolume);
        PlayerPrefs.SetFloat("masterVolume", masterVolume);
    }
    public void SetMusicVolume(float musicVolume)
    {
        audioMixer.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
    }
    public void SetEffectsVolume(float effectsVolume)
    {
        audioMixer.SetFloat("effectsVolume", effectsVolume);
        PlayerPrefs.SetFloat("effectsVolume", effectsVolume);
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
