using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardsScript : MonoBehaviour
{
    public TextMeshProUGUI personalBestText;

    // Start is called before the first frame update
    void Start()
    {
        float personalBest = PlayerPrefs.GetFloat("BestTime", float.MaxValue);
        int minutes = Mathf.FloorToInt(personalBest / 60);
        int seconds = Mathf.FloorToInt(personalBest % 60);
        int milliseconds = Mathf.FloorToInt((personalBest - Mathf.Floor(personalBest)) * 1000);

        if (minutes > 0)
        {
            personalBestText.text = string.Format("Personal Best: {0:D2}:{1:D2}.{2:D3}", minutes, seconds, milliseconds);
        }
        else
        {
            personalBestText.text = string.Format("Personal Best: {0:D2}.{1:D3}", seconds, milliseconds);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
