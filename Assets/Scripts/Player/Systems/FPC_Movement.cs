using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPC_Movement : MonoBehaviour
{
    [SerializeField, Min(0.1f)]
    private float speed = 11f;
    [SerializeField, Range(0, 1)]
    private float inAirMoveForce = 0;
    [SerializeField]
    private float gravity = -30f;
    [SerializeField]
    private LayerMask groundCheckIgnoreMask;
    [SerializeField, Min(0.1f)]
    private float jumpHeight = 1;
    [SerializeField, Min(1.1f)]
    private float sprintMultiplier = 2;
    [SerializeField, Min(1)]
    private float sprintTime = 10;

    private CharacterController characterController;
    private Vector2 horizontalInput;
    private Vector2 verticalVelocity;
    private Vector3 horizontalVelocity;
    private bool isGrounded;
    private bool jump;
    private bool sprint;
    private float currentSprintTime = 0;
    private bool sprintRegen = false;
    private bool crouch;

    public event Action<float, float, bool> SprintStatusChanged;

    private void Awake()
    {
        currentSprintTime = sprintTime;
        characterController = GetComponent<CharacterController>();
    }

    public void ReceiveInput(Vector2 _horizontalInput)
    {
        horizontalInput = _horizontalInput;
    }

    public void OnJumpPressed()
    {
        jump = true;
    }

    public void SprintToggle()
    {
        sprint = !sprint;
        if (sprint)
        {
            SprintStatusChanged?.Invoke(currentSprintTime, sprintTime, true);
            sprintRegen = true;
        }
    }

    public void CrouchToggle()
    {

    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position, 0.1f, ~groundCheckIgnoreMask);

        if(!sprint && sprintRegen)
        {
            currentSprintTime += Time.deltaTime;
            SprintStatusChanged?.Invoke(currentSprintTime, sprintTime, true);
            if (currentSprintTime >= sprintTime)
            {
                SprintStatusChanged?.Invoke(currentSprintTime, sprintTime, false);
                currentSprintTime = sprintTime;
                sprintRegen = false;
 
            }
        }

        if (isGrounded)
        {
            verticalVelocity = Vector2.zero;
            horizontalVelocity = transform.right * horizontalInput.x + transform.forward * horizontalInput.y;
            if(sprint)
            {
                currentSprintTime -= Time.deltaTime;
                SprintStatusChanged?.Invoke(currentSprintTime, sprintTime, true);
                horizontalVelocity *= 2;
                if(currentSprintTime < 0)
                {
                    SprintToggle();
                }
            }
        }
        else
        {
            horizontalVelocity += (transform.right * horizontalInput.x + transform.forward * horizontalInput.y)
                *inAirMoveForce;
            horizontalVelocity.Normalize();
        }

        horizontalVelocity *= speed;
        

        characterController.Move(horizontalVelocity * Time.deltaTime);

        if(jump)
        {
            if (isGrounded)
            {
                verticalVelocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
            }
            jump = false;
        }

        verticalVelocity.y += gravity * Time.deltaTime;
        characterController.Move(verticalVelocity * Time.deltaTime);
    }
}
