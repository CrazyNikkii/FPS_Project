using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMode : MonoBehaviour
{
    public bool agilityCompleted = false;

    void Start()
    {
        
        agilityCompleted = PlayerPrefs.GetInt("AgilityCompleted") == 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AgilityCompleted()
    {
        Debug.Log("AgilityCompleted");
        agilityCompleted = true;
        PlayerPrefs.SetInt("AgilityCompleted", 1);
    }
}
