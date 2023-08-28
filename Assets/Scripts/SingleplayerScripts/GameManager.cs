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

    // Training GameMode
    public GameObject trainingDummies;
    public int numberOfDummies;
    public GameObject spawnWallWest;
    public GameObject spawnWallEast;
    public int enemiesLeft;
    public float startTimer = 5f;
    public TextMeshProUGUI startTimerText;
    public TimerScript timerScript;
    public TextMeshProUGUI enemiesLeftText;
    public bool trainingModeEnded = false;
    public bool trainingModeRestartable;

    // Pause
    public bool gamePaused = false;
    public GameObject pauseMenuUI;
    public GameObject settingsUI;
    public GameObject hud;

void Start()
    {
        spawnWallEast.SetActive(true);
        spawnWallWest.SetActive(true);
        trainingModeRestartable = false;
        timerScript = FindObjectOfType<TimerScript>();
        enemiesLeft = numberOfDummies;
        startTimer = 5;
        // Destroys spawnpoint dummies
        var dummys = GameObject.FindGameObjectsWithTag("Dummy");
        foreach (var dummy in dummys)
        {
            Destroy(dummy);
        }

        // Pause status
        gamePaused = false;
        Time.timeScale = 1f;
        Scene scene = SceneManager.GetActiveScene();
        if(scene.name == "MainMenu")
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        // Gamemode start text off
        startTimerText.gameObject.SetActive(false);
    }


    void Update()
    {
        enemiesLeftText.text = enemiesLeft.ToString() + ": Left";
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

        if (enemiesLeft <= 0 && !trainingModeEnded)
        {
            TrainingModeEnd();
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

    public void TrainingTestStart()
    {
        StartCoroutine(TrainingModeStartRoutine());
        startTimerText.gameObject.SetActive(true);
        trainingModeRestartable = false;
        spawnWallEast.SetActive(true);
        spawnWallWest.SetActive(true);
    }

    public IEnumerator TrainingModeStartRoutine()
    {
        while (startTimer > 0)
        {
            startTimerText.text = startTimer.ToString("0");
            yield return new WaitForSeconds(1f);
            startTimer--;
        }
        TrainingModeStart();
        Debug.Log("Enemies left: " + enemiesLeft);
        startTimerText.gameObject.SetActive(false);
    }

    // Training GameMode
    public void TrainingModeStart()
    {
        Debug.Log("trainingmodestart called");
        // Spawn Dummies
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("DummySpawnPoint");
        if (spawnPoints.Length < numberOfDummies )
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

        // Remove spawn walls
        spawnWallEast.SetActive(false);
        spawnWallWest.SetActive(false);

        timerScript.StartTimer();
        TrainingMode();
    }

    public void TrainingMode()
    {
        Debug.Log("TrainingMode called. enemiesLeft:" + enemiesLeft);
    }

    public void TrainingModeEnd()
    {
        Debug.Log("TrainingMode ending");
        trainingModeEnded = true;
        timerScript.timerRunning = false;
        timerScript.StopTimer();
        trainingModeRestartable = true;
        startTimer = 5;
    }

    public void RestartScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
