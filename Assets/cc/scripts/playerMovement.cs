using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;

    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
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
        characterController.SimpleMove(movementDirection * magnitude);
        //transform.Translate(movementDirection * magnitude * Time.deltaTime * speed, Space.World);
        
        // move charactor face to the transform direction
        if(movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
