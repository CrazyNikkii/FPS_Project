using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerScript : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private bool timerRunning = false;
    private float startTime;

    public void StartTimer()
    {
        timerRunning = true;
        startTime = Time.time;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }
    private void Update()
    {
        if (timerRunning)
        {
            float currentTime = Time.time - startTime;
            timerText.text = currentTime.ToString("F2");
        }
    }
}
