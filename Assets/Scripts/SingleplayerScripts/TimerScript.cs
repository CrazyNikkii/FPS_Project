using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerScript : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI bestTimeText;
    public bool timerRunning = false;
    public float startTime;
    public float endTime;
    public GameManager gm;

    public void Start()
    {
        gm = FindObjectOfType<GameManager>();
        UpdateBestTime();
    }

    public void StartTimer()
    {
        timerRunning = true;
        startTime = Time.time;
    }

    public void StopTimer()
    {
        timerRunning = false;
        endTime = Time.time;
        DisplayFinalTime();
        UpdateBestTime();
    }

    public void Update()
    {
        if (timerRunning)
        {
            float currentTime = Time.time - startTime;

            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            int milliseconds = Mathf.FloorToInt((currentTime - Mathf.Floor(currentTime)) * 1000);

            if (minutes > 0)
            {
                string formattedTime = string.Format("{0:D2}:{1:D2}.{2:D3}", minutes, seconds, milliseconds);
                timerText.text = formattedTime;
            }
            else
            {
                string formattedTime = string.Format("{0:D2}.{1:D3}", seconds, milliseconds);
                timerText.text = formattedTime;
            }
        }
    }

    public void DisplayFinalTime()
    {
        if (!timerRunning)
        {
            float elapsedTime = endTime - startTime;

            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            int milliseconds = Mathf.FloorToInt((elapsedTime - Mathf.Floor(elapsedTime)) * 1000);

            if (minutes > 0)
            {
                string formattedTime = string.Format("{0:D2}:{1:D2}.{2:D3}", minutes, seconds, milliseconds);
                timerText.text = formattedTime;
            }
            else
            {
                string formattedTime = string.Format("{0:D2}.{1:D3}", seconds, milliseconds);
                timerText.text = formattedTime;
            }
        }
    }

    public void UpdateBestTime()
    {
        float elapsedTime = endTime - startTime;

        float bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);
        if (elapsedTime < bestTime)
        {
            bestTime = elapsedTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
            UpdateBestTimeUI(bestTime);
        }
    }

    public void UpdateBestTimeUI(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        int milliseconds = Mathf.FloorToInt((timeInSeconds - Mathf.Floor(timeInSeconds)) * 1000);

        if (minutes > 0)
        {
            bestTimeText.text = string.Format("Best Time: {0:D2}:{1:D2}.{2:D3}", minutes, seconds, milliseconds);
        }
        else
        {
            bestTimeText.text = string.Format("Best Time: {0:D2}.{1:D3}", seconds, milliseconds);
        }
    }
}


