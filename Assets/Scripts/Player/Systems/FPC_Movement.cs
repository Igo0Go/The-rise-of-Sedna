using System;
using System.Collections;
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
    [SerializeField, Min(0.1f)]
    private float crouchHeight = 0.5f;
    [SerializeField, Min(0.1f)]
    private float crouchCenterHeight = 0.5f;
    [SerializeField, Min(0.1f)]
    private float crouchTransitionSpeed = 10f;
    [SerializeField]
    private Transform cameraPoint;
    [SerializeField]
    private Transform crouchCameraPoint;
    [SerializeField]
    private Transform standCameraPoint;
    [SerializeField]
    private FPC_HeadbobSystem fPC_HeadbobSystem;

    private CharacterController characterController;
    private Vector2 horizontalInput;
    private Vector2 verticalVelocity;
    private Vector3 horizontalVelocity;
    private bool isGrounded;
    private bool jump;
    private bool useSprint;
    private float currentSprintTime = 0;
    private bool sprintRegen = false;

    private bool isCrouching = false;
    private float originalHeight;
    private float originalCenterHeight;


    public event Action<float, float, bool> SprintStatusChanged;

    private void Awake()
    {
        currentSprintTime = sprintTime;
        characterController = GetComponent<CharacterController>();
        originalHeight = characterController.height;
        originalCenterHeight = characterController.center.y;
    }

    public void ReceiveInput(Vector2 _horizontalInput)
    {
        horizontalInput = _horizontalInput;
        if (isGrounded)
        {
            fPC_HeadbobSystem.SetInput(horizontalInput);
        }
        else
        {
            fPC_HeadbobSystem.SetInput(Vector2.zero);
        }

        if(horizontalInput == Vector2.zero && useSprint)
        {
            SprintToggle();
        }
    }

    public void OnJumpPressed()
    {
        jump = true;
    }

    public void SprintToggle()
    {
        useSprint = !useSprint;
        if (useSprint)
        {
            if(CheckSurfaceAboveHead())
            {
                useSprint = false;
                return;
            }

            if (isCrouching)
            {
                CrouchToggle();
            }

            SprintStatusChanged?.Invoke(currentSprintTime, sprintTime, true);
            sprintRegen = true;
        }
    }

    public void CrouchToggle()
    {
        isCrouching = !isCrouching;

        if (isCrouching)
        {
            if(useSprint)
            {
                SprintToggle();
            }
            StopAllCoroutines();
            StartCoroutine(ToCrouchCoroutine());
        }
        else
        {
            if (CheckSurfaceAboveHead())
            {
                isCrouching = true;
                return;
            }

            StopAllCoroutines();
            StartCoroutine(ToStandCoroutine());
        }
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position, 0.1f, ~groundCheckIgnoreMask);

        if(isCrouching)
        {
            fPC_HeadbobSystem.SetFrequency(0.7f);
            fPC_HeadbobSystem.SetAmount(0.2f);
        }
        else if (useSprint)
        {
            fPC_HeadbobSystem.SetFrequency(1.5f);
            fPC_HeadbobSystem.SetAmount(2f);
        }
        else
        {
            fPC_HeadbobSystem.SetFrequency(1f);
            fPC_HeadbobSystem.SetAmount(1f);
        }


        if (!useSprint && sprintRegen)
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
            if(useSprint)
            {
                currentSprintTime -= Time.deltaTime;
                SprintStatusChanged?.Invoke(currentSprintTime, sprintTime, true);
                horizontalVelocity *= sprintMultiplier;
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
        if(isCrouching)
        {
            horizontalVelocity /= 2;
        }

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

    private IEnumerator ToCrouchCoroutine()
    {
        float t = 0;
        while (t < 1)
        {
            t+= Time.deltaTime * crouchTransitionSpeed;
            cameraPoint.localPosition = Vector3.Lerp(standCameraPoint.localPosition, crouchCameraPoint.localPosition, t);
            yield return null;
        }
        characterController.height = crouchHeight;
        characterController.center = new Vector3(0,crouchCenterHeight, 0);
    }
    private IEnumerator ToStandCoroutine()
    {
        float t = 1;
        while (t > 0)
        {
            t -= Time.deltaTime * crouchTransitionSpeed;
            cameraPoint.localPosition = Vector3.Lerp(standCameraPoint.localPosition, crouchCameraPoint.localPosition, t);
            yield return null;
        }
        characterController.height = originalHeight;
        characterController.center = new Vector3(0, originalCenterHeight, 0);
    }

    private bool CheckSurfaceAboveHead()
    {
        if(Physics.Raycast(cameraPoint.position, transform.up, originalHeight/2, ~groundCheckIgnoreMask))
        {
            return true;
        }
        return false;
    }
}
