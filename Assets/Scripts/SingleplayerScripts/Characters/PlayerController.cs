using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public class PlayerController : MonoBehaviour
{
    // Player stats
    [SerializeField] private float walkSpeed = 7.5f;
    [SerializeField] private float runSpeed = 11.5f;
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float gravity = 20.0f;
    public float sensitivity;
    public float cameraLimit = 90.0f;
    public float interactionDistance = 3f;
    
    // References
    public GameManager gm;
    public Camera playerCam;
    public LayerMask mask;
    CharacterController characterController;

    // States
    public bool isScoped;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0f;
    public bool canMove;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Block movement during pause
        canMove = !gm.gamePaused;

        // Sets correct sensitivty between scoped and normal
        if(isScoped)
        {
            sensitivity = PlayerPrefs.GetFloat("ScopedSensitivity");
        }
        else
        {
            sensitivity = PlayerPrefs.GetFloat("Sensitivity");
        }
        

        // Moving
        Vector3 forward = transform.TransformDirection (Vector3.forward);
        Vector3 right = transform.TransformDirection (Vector3.right);

        bool isRunning = Input.GetKey (KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis ("Vertical") : 0f;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis ("Horizontal") : 0f;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Jump
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpForce;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move (moveDirection * Time.deltaTime);

        // Mouse Movement
        if(canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * sensitivity;
            rotationX = Mathf.Clamp(rotationX, -cameraLimit, cameraLimit);
            playerCam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * sensitivity, 0);
        }

        // Interact
        gm.UpdateText(string.Empty);
        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * interactionDistance);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, interactionDistance, mask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                gm.UpdateText(interactable.promptMessage);
                if(Input.GetKeyDown(KeyCode.E))
                {
                    interactable.BaseInteract();
                }
            }
        }
    }
}
