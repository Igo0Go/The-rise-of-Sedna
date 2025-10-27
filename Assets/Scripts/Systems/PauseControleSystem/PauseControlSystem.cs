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
    public bool IsDead
    {
        get
        {
            return _isDead;
        }
        set
        {
            _isDead = value;
            PauseUpdate();
        }
    }
    private bool _isDead = false;

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
        SetTimeBlockState(_isMessage || _isDead);
        SetCotrollBlockState(_isMessage || _isCutscene || _isDead);
        HudActiveChanged?.Invoke(!_isCutscene || !_isDead);
        CursorModeChanged?.Invoke(_isMessage || _isDead);
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
