using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // FPS counter
    public TextMeshProUGUI fpsText;
    private float pollingTime = 1f;
    private float time;
    private int frameCount;

    // Interactions
    public TextMeshProUGUI promptText;

    // Pause
    public bool gamePaused = false;
    public GameObject pauseMenuUI;
    public GameObject settingsUI;
    public GameObject hud;

    public WeaponSwitcher weaponSwitcher;
    public GameObject weaponHolder;

void Start()
    {
        weaponHolder = GameObject.FindGameObjectWithTag("GunHolder");
        weaponSwitcher = weaponHolder.GetComponent<WeaponSwitcher>();
        // Pause status
        gamePaused = false;
        Time.timeScale = 1f;
        Scene scene = SceneManager.GetActiveScene();
        if(scene.name == "MainMenu")
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        if(scene.name == "SingleplayerMap")
        {
            weaponSwitcher.allowSwitch = true;
        }
    }


    void Update()
    {
        // FPS counter
        time += Time.deltaTime;
        frameCount++;
        if(time >= pollingTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount/time);
            fpsText.text = frameRate.ToString() + " FPS";
        }

        // Cursos lockstate
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
        hud.SetActive(false);
        Time.timeScale = 0f;
        gamePaused = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        hud.SetActive(true);
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

    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }

    public void RestartScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
