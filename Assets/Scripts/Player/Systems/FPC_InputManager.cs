using UnityEngine;

public class FPC_InputManager : MonoBehaviour
{
    [SerializeField]
    private FPC_Movement fPC_Movement;
    [SerializeField]
    private FPC_View fPC_Veiew;
    [SerializeField]
    private FPC_WeaponSystem fPC_WeaponSystem;
    [SerializeField]
    private FPC_Interaction fPC_Interaction;

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
        groundMovement.Attack.performed += _ => fPC_WeaponSystem.MainAttack();
        groundMovement.Attack.canceled += _ => fPC_WeaponSystem.StopMainAttack();
        groundMovement.Reload.performed += _ => fPC_WeaponSystem.StartReload();
        groundMovement.Reload.canceled += _ => fPC_WeaponSystem.StopReload();
        groundMovement.Action.performed += _ => fPC_Interaction.OnUseInput();
        groundMovement.Jump.performed += _ => fPC_Movement.OnJumpPressed();
        groundMovement.Sprint.performed += _ => fPC_Movement.SprintToggle();
        groundMovement.Crouch.performed += _ => fPC_Movement.CrouchToggle();


        Application.targetFrameRate = 60;
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
    }
}
