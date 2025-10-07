using UnityEngine;
using System;

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
    public bool IsCutscene
    {
        get
        {
            return _isCutscene;
        }
        set
        {
            _isCutscene = value;
            PauseUpdate();
        }
    }
    private bool _isCutscene = false;

    [SerializeField]
    private FPC_InputManager _inputManager;

    public event Action<bool> CursorModeChanged;
    public event Action<bool> HudActiveChanged;

    private void Start()
    {
        PauseUpdate();
    }

    private void PauseUpdate()
    {
        SetTimeBlockState(_isMessage);
        SetCotrollBlockState(_isMessage || _isCutscene);
        HudActiveChanged?.Invoke(!_isCutscene);
        CursorModeChanged?.Invoke(_isMessage);
    }

    private void SetCotrollBlockState(bool value)
    {
        _inputManager.ResetInput();
        _inputManager.enabled = !value;
    }
    private void SetTimeBlockState(bool value)
    {
        Time.timeScale = value ? 0 : 1;
    }
}
