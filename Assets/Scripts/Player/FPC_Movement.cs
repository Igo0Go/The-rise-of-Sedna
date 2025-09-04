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


    private CharacterController characterController;
    private Vector2 horizontalInput;
    private Vector2 verticalVelocity;
    private Vector3 horizontalVelocity;
    private bool isGrounded;
    private bool jump;

    private void Awake()
    {
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

    private void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position, 0.1f, ~groundCheckIgnoreMask);

        if (isGrounded)
        {
            verticalVelocity = Vector2.zero;
            horizontalVelocity = transform.right * horizontalInput.x + transform.forward * horizontalInput.y;
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
