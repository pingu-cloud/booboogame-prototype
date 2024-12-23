using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public VariableJoystick joystick;
    public Canvas inputCanvas;
    public CharacterController controller;
    public float movementSpeed;
    public bool isJoystick;
    public float rotationSpeed;
    public Animator animator;

    private void Start()
    {
        EnableJoystickInput();
    }

    public void EnableJoystickInput()
    {
        isJoystick = true;
        inputCanvas.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (isJoystick)
        {
            Vector3 movementDirection = new Vector3(joystick.Direction.x, 0.0f, joystick.Direction.y);
            if (movementDirection.magnitude > 1)
            {
                
                movementDirection.Normalize();
            }
            controller.Move(movementDirection * movementSpeed * Time.deltaTime);
            if(movementDirection.sqrMagnitude<=0)
            {
                animator.SetBool("IsWalking", false);
                return;
            }
            animator.SetBool("IsWalking", true);
            var targetDirection = Vector3.RotateTowards(controller.transform.forward, movementDirection, rotationSpeed * Time.deltaTime, 0.0f);
            controller.transform.rotation=Quaternion.LookRotation(targetDirection);
        }
    }
}
