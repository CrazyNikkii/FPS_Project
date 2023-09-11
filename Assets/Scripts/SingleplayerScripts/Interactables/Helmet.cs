using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helmet : Interactable
{

    public bool interactedOnce;
    public GameObject hUD;
    void Start()
    {
        interactedOnce = false;
    }

    protected override void Interact()
    {
        if(!interactedOnce)
        {
            hUD.SetActive(true);
            interactedOnce = true;
            Destroy(gameObject);
            Debug.Log("Interacted with " + gameObject.name);
        }
        else
        {
            return;
        }
    }
}
