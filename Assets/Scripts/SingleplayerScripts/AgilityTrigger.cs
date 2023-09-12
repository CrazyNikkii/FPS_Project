using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgilityTrigger : MonoBehaviour
{
    public TutorialMode tutorialMode;

    public void Start()
    {
        tutorialMode = FindObjectOfType<TutorialMode>();
    }
    public void OnTriggerEnter()
    {
        tutorialMode.AgilityCompleted();
    }
}
