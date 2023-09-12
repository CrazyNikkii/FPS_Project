using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : Interactable
{
    public GameObject door;
    public TutorialMode tutorialMode;

    public bool doorOpen;

    void Start()
    {
        tutorialMode = FindObjectOfType<TutorialMode>();
    }

    protected override void Interact()
    {
        doorOpen = !doorOpen;
        door.GetComponent<Animator>().SetBool("IsOpen", doorOpen);
        tutorialMode.AgilityCompleted();
    }
}
