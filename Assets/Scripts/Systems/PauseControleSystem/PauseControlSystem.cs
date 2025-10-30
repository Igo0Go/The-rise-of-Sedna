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
    public bool IsJornal
    {
        get
        {
            return _isJornal;
        }
        set
        {
            _isJornal = value;
            PauseUpdate();
        }
    }
    private bool _isJornal = false;
    public bool IsSkillPanel
    {
        get
        {
            return _isSkillPanel;
        }
        set
        {
            _isSkillPanel = value;
            PauseUpdate();
        }
    }
    private bool _isSkillPanel = false;

    public bool MenuWindow => IsMessage || IsDead || IsJornal || IsSkillPanel;

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
        SetTimeBlockState(MenuWindow);
        SetCotrollBlockState(MenuWindow || _isCutscene);
        HudActiveChanged?.Invoke(!_isCutscene || !_isDead);
        CursorModeChanged?.Invoke(MenuWindow);
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
