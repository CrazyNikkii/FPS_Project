using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerScript : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public bool timerRunning = false;
    public float startTime;


    public void StartTimer()
    {
        timerRunning = true;
        startTime = Time.time;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }
    public void Update()
    {
        if (timerRunning)
        {
            float currentTime = Time.time - startTime;
            int seconds = Mathf.FloorToInt(currentTime);
            int tenths = Mathf.FloorToInt((currentTime - seconds) * 10);
            int hundreds = Mathf.FloorToInt((currentTime - seconds - tenths * 0.1f) * 100);

            string formattedTime = string.Format("{0:D2}:{1:D1}{2:D1}", seconds, tenths, hundreds);
            timerText.text = formattedTime;
        }
    }
}
