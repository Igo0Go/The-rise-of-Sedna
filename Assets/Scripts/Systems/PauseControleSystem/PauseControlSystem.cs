using UnityEngine;

public class PauseControlSystem : MonoBehaviour
{
    public bool IsMessage
    {
        get
        {
            return _isMessage;
        }
        set
        {
            _isMessage = value;
            PauseUpdate();
        }
    }
    private bool _isMessage = false;

    [SerializeField]
    private FPC_InputManager _inputManager;

    private void Awake()
    {
        PauseUpdate();
    }

    private void PauseUpdate()
    {
        bool pause = _isMessage;
        SetPause(pause);
    }

    private void SetPause(bool value)
    {
        Time.timeScale = value ? 0 : 1;
        Cursor.lockState = value? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = value;
        _inputManager.ResetInput();
        _inputManager.enabled = !value;
    }
}
