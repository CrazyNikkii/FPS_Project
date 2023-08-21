using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
public class CameraController : NetworkBehaviour
{
    public GameObject playerCamObject;

    public Camera playerCam;

    void Start()
    {
        playerCam = playerCamObject.GetComponent<Camera>();
        if(!IsOwner)
        {
            playerCam.enabled = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
