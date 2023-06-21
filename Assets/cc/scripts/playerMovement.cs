using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float maxSpeed;
    public float rotationSpeed;
    public float jumpSpeed;
    public float jumpButtonGracePeriod;

    [SerializeField]
    private Transform cameraTransform;

    [SerializeField]
    private CinemachineFreeLook cinemachineFreeLook;

    private CharacterController characterController;
    private float ySpeed;
    private float orignalStepOffset;
    private float? lastGroundTime;
    private float? jumpButtonPressedTime;
    private Animator animator;


    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        orignalStepOffset = characterController.stepOffset;
    }

    void Update()
    {
        // Get value from key/Gamepad
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // get movement direction in vector and calculate magnitude to transform the charactor
        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);
        

       
       
        //animator.SetFloat("Input Magnitude", inputMagnitude);
        float speed = inputMagnitude * maxSpeed;
        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection    ;
        movementDirection.Normalize();

        Vector3 velocity = movementDirection * speed;
        velocity.y = ySpeed;
        characterController.Move(velocity * Time.deltaTime);
        

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

        if (Input.GetMouseButton(0))
        {
            cinemachineFreeLook.m_XAxis.m_MaxSpeed = 50;
        }
        else
        {
            cinemachineFreeLook.m_XAxis.m_MaxSpeed = 0;
        }
        // move charactor face to the transform direction
        if(movementDirection != Vector3.zero)
        {
            
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, Time.deltaTime * rotationSpeed);
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
            }
            
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
    }
}
