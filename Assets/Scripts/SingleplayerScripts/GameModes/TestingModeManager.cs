using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestingModeManager : MonoBehaviour
{

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
    public GameObject dummyContainer;

    public void Start()
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

        // Gamemode start text off
        startTimerText.gameObject.SetActive(false);
    }

    public void Update()
    {
        enemiesLeftText.text = enemiesLeft.ToString() + ": Left";

        if (enemiesLeft <= 0 && !trainingModeEnded)
        {
            TrainingModeEnd();
        }

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
        if (spawnPoints.Length < numberOfDummies)
        {
            Debug.LogWarning("Not enough spawnpoints for the dummies");
            return;
        }

        System.Random dummyRNG = new System.Random();
        int n = spawnPoints.Length;

        while (n > 1)
        {
            n--;
            int k = dummyRNG.Next(n + 1);
            GameObject value = spawnPoints[k];
            spawnPoints[k] = spawnPoints[n];
            spawnPoints[n] = value;
        }

        for (int i = 0; i < numberOfDummies; i++)
        {
            GameObject dummies = Instantiate(trainingDummies, spawnPoints[i].transform.position, Quaternion.identity);
            dummies.transform.SetParent(dummyContainer.transform);
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
}
