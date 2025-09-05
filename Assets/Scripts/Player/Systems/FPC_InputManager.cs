using UnityEngine;

public class FPC_InputManager : MonoBehaviour
{
    [SerializeField]
    private FPC_Movement fPC_Movement;
    [SerializeField]
    private FPC_View fPC_Veiew;

    private FPC controls;
    private FPC.GroundMovementActions groundMovement;

    private Vector2 horizontalInput;
    private Vector2 viewInput;

    private void Awake()
    {
        controls = new FPC();
        groundMovement = controls.GroundMovement;

        groundMovement.HoriozontalMovement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();
        groundMovement.View.performed += ctx => viewInput = ctx.ReadValue<Vector2>();
        groundMovement.Jump.performed += _ => fPC_Movement.OnJumpPressed();
        groundMovement.Sprint.performed += _ => fPC_Movement.SprintToggle();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        fPC_Movement.ReceiveInput(horizontalInput);
        fPC_Veiew.ReceiveInput(viewInput);

        if(Input.GetMouseButtonDown(0))
        {
            Application.targetFrameRate = 30;
        }
        if (Input.GetMouseButtonDown(1))
        {
            Application.targetFrameRate = 60;
        }
        if (Input.GetMouseButtonDown(2))
        {
            Application.targetFrameRate = 120;
        }
    }
}
