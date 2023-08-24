using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Configuration;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    public TextMeshProUGUI promptText;
    private float pollingTime = 1f;
    private float time;
    private int frameCount;

    // Training GameMode
    public GameObject trainingDummies;
    public int numberOfDummies = 10;

    // Pause
    public bool gamePaused = false;
    public GameObject pauseMenuUI;
    public GameObject settingsUI;

    void Start()
    {
        var dummys = GameObject.FindGameObjectsWithTag("Dummy");
        foreach (var dummy in dummys)
        {
            Destroy(dummy);
        }
        TrainingModeStart();
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

    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }

    // Training GameMode
    public void TrainingModeStart()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("DummySpawnPoint");
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("Not enough spawnpoints for the dummies");
            return;
        }

        System.Random dummyRNG = new System.Random();
        int n = spawnPoints.Length;

        while(n > 1)
        {
            n--;
            int k = dummyRNG.Next(n +1);
            GameObject value = spawnPoints[k];
            spawnPoints[k] = spawnPoints[n];
            spawnPoints[n] = value;
        }

        for(int i = 0; i < numberOfDummies; i++)
        {
            Instantiate(trainingDummies, spawnPoints[i].transform.position, Quaternion.identity);
        }
    }
}
