using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPC_Movement : MonoBehaviour
{
    [SerializeField]
    private float gravity = -30f;
    [SerializeField]
    private LayerMask groundCheckIgnoreMask;
    [SerializeField, Min(0.1f)]
    private float crouchHeight = 0.5f;
    [SerializeField, Min(0.1f)]
    private float crouchCenterHeight = 0.5f;
    [SerializeField, Min(0.01f)]
    private float groundCheckSphereRadius = 10f;
    [SerializeField]
    private Transform cameraPoint;
    [SerializeField]
    private Transform crouchCameraPoint;
    [SerializeField]
    private Transform standCameraPoint;
    [SerializeField]
    private FPC_HeadbobSystem fPC_HeadbobSystem;
    [SerializeField]
    private FootStepSystem footstepSystem;

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
    private float crouchTransitionSpeed = 8f;
    private PlayerSoundType currentSoundType;

    public event Action<float, float, bool> SprintStatusChanged;

    private void Start()
    {
        currentSprintTime = SkillHolder.Instance.sprintTime.currentValue;
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

        if(horizontalInput == Vector2.zero)
        {
            footstepSystem.ResetStepValue();
        }
        else
        {
            footstepSystem.Step(currentSoundType, transform);
            float stepDelta = SkillHolder.Instance.speed.currentValue;
            if(useSprint)
            {
                stepDelta *= SkillHolder.Instance.sprintMultiplier.currentValue;
            }
            footstepSystem.AddStepValue(stepDelta * Time.deltaTime);
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

            SprintStatusChanged?.Invoke(currentSprintTime, SkillHolder.Instance.sprintTime.currentValue, true);
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
        isGrounded = Physics.CheckSphere(transform.position, groundCheckSphereRadius, ~groundCheckIgnoreMask);

        if(isCrouching)
        {
            fPC_HeadbobSystem.SetFrequency(0.7f);
            fPC_HeadbobSystem.SetAmount(0.2f);
            currentSoundType = PlayerSoundType.crouchMove;
        }
        else if (useSprint)
        {
            fPC_HeadbobSystem.SetFrequency(1.5f);
            fPC_HeadbobSystem.SetAmount(2f);
            currentSoundType = PlayerSoundType.runMove;
        }
        else
        {
            fPC_HeadbobSystem.SetFrequency(1f);
            fPC_HeadbobSystem.SetAmount(1f);
            currentSoundType = PlayerSoundType.simpleMove;
        }


        if (!useSprint && sprintRegen)
        {
            currentSprintTime += Time.deltaTime;
            SprintStatusChanged?.Invoke(currentSprintTime, SkillHolder.Instance.sprintTime.currentValue, true);
            if (currentSprintTime >= SkillHolder.Instance.sprintTime.currentValue)
            {
                SprintStatusChanged?.Invoke(currentSprintTime, SkillHolder.Instance.sprintTime.currentValue, false);
                currentSprintTime = SkillHolder.Instance.sprintTime.currentValue;
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
                SprintStatusChanged?.Invoke(currentSprintTime, SkillHolder.Instance.sprintTime.currentValue, true);
                horizontalVelocity *= SkillHolder.Instance.sprintMultiplier.currentValue;
                if(currentSprintTime < 0)
                {
                    SprintToggle();
                }
            }
        }
        else
        {
            horizontalVelocity += (transform.right * horizontalInput.x + transform.forward * horizontalInput.y)
                * SkillHolder.Instance.inAirMoveForce.currentValue;
            horizontalVelocity.Normalize();
        }

        horizontalVelocity *= SkillHolder.Instance.speed.currentValue;
        if(isCrouching)
        {
            horizontalVelocity /= 2;
        }

        characterController.Move(horizontalVelocity * Time.deltaTime);

        if(jump)
        {
            if (isGrounded)
            {
                verticalVelocity.y = Mathf.Sqrt(-2f * SkillHolder.Instance.jumpHeight.currentValue * gravity);
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
            cameraPoint.localPosition = Vector3.Lerp(standCameraPoint.localPosition, 
                crouchCameraPoint.localPosition, t);
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
