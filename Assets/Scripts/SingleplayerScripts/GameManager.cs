using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Configuration;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    private float pollingTime = 1f;
    private float time;
    private int frameCount;

    // Pause
    public bool gamePaused = false;
    public GameObject pauseMenuUI;
    public GameObject settingsUI;

    void Start()
    {
        gamePaused = false;
        Time.timeScale = 1f;
        Scene scene = SceneManager.GetActiveScene();
        if(scene.name == "MainMenu")
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }


    void Update()
    {
        time += Time.deltaTime;
        frameCount++;
        if(time >= pollingTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount/time);
            fpsText.text = frameRate.ToString() + " FPS";
        }

        if(gamePaused == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Pausing
        if(Input.GetKeyDown(KeyCode.Escape) && settingsUI.activeInHierarchy == false)
        {
            if(gamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SaveData();
    }

    public void SaveData()
    {
        PlayerPrefs.Save();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        SaveData();
    }
}
