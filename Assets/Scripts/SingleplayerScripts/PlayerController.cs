using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public class PlayerController : MonoBehaviour
{
    //Floats and stuff
    [SerializeField] private float walkSpeed = 7.5f;
    [SerializeField] private float runSpeed = 11.5f;
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float gravity = 20.0f;
    public Camera playerCam;
    public float sensitivity;
    public float cameraLimit = 45.0f;
    public GameManager gm;
    public float interactionDistance = 3f;
    public LayerMask mask;



    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0f;
    public bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        float sens = PlayerPrefs.GetFloat("Sensitivity", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        canMove = !gm.gamePaused;
        sensitivity = PlayerPrefs.GetFloat ("Sensitivity");

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

        gm.UpdateText(string.Empty);
        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * interactionDistance);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, interactionDistance, mask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                gm.UpdateText(hitInfo.collider.GetComponent<Interactable>().promptMessage);
            }
        }
    }
}
