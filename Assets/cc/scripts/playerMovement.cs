using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public float jumpSpeed;
    public float jumpButtonGracePeriod;

    private CharacterController characterController;
    private float ySpeed;
    private float orignalStepOffset;
    private float? lastGroundTime;
    private float? jumpButtonPressedTime;


    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        orignalStepOffset = characterController.stepOffset;
    }

    void Update()
    {
        // Get value from key/Gamepad
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // get movement direction in vector and calculate magnitude to transform the charactor
        Vector3 movementDirection = new Vector3(horizontalInput,0, verticalInput);
        float magnitude = movementDirection.magnitude;
        magnitude = Mathf.Clamp01((float)magnitude);
        movementDirection.Normalize();
        Vector3 velocity = movementDirection * magnitude;
        velocity.y = ySpeed;
        characterController.Move(velocity * Time.deltaTime);
        //transform.Translate(movementDirection * magnitude * Time.deltaTime * speed, Space.World);

        //jump Character
        ySpeed += Physics.gravity.y * Time.deltaTime;
        if (characterController.isGrounded)
        {
            lastGroundTime = Time.time;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpButtonPressedTime = Time.time;
        }
        if (Time.time - lastGroundTime <= jumpButtonGracePeriod)
        {
            characterController.stepOffset = orignalStepOffset;
            ySpeed = -0.5f;
            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                ySpeed = jumpSpeed;
                jumpButtonPressedTime = null;
                lastGroundTime = null;
            }
        }
        else
        {
            characterController.stepOffset = 0f;
        }
        
        // move charactor face to the transform direction
        if(movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
