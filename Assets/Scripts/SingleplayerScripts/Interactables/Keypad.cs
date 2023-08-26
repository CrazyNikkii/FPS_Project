using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : Interactable
{
    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Interact()
    {
        if(gm.trainingModeStartable)
        {
            gm.TrainingTestStart();
            Debug.Log("Interacted with " + gameObject.name);
        }

    }
}
