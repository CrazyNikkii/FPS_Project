using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : Interactable
{
    public TestingModeManager testingModeManager;
    public GameManager gm;
    public bool interactedOnce;

    // Start is called before the first frame update
    void Start()
    {
        testingModeManager = FindObjectOfType<TestingModeManager>();
        gm = FindObjectOfType<GameManager>();
        interactedOnce = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Interact()
    {
        if(!testingModeManager.trainingModeRestartable && !interactedOnce)
        {
            testingModeManager.TrainingTestStart();
            interactedOnce = true;
            Debug.Log("Interacted with " + gameObject.name);
        }
        else if (interactedOnce && testingModeManager.trainingModeRestartable)
        {
            gm.RestartScene();
        }
        else
        {
            return;
        }
    }
}
