using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : Interactable
{
    public GameManager gm;
    public bool interactedOnce;

    // Start is called before the first frame update
    void Start()
    {
        interactedOnce = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Interact()
    {
        if(!gm.trainingModeRestartable && !interactedOnce)
        {
            gm.TrainingTestStart();
            interactedOnce = true;
            Debug.Log("Interacted with " + gameObject.name);
        }
        else if (interactedOnce && gm.trainingModeRestartable)
        {
            gm.RestartScene();
        }
        else
        {
            return;
        }
    }
}
