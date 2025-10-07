using UnityEngine;

public class FPC_HUDController : MonoBehaviour
{
    [SerializeField]
    private GameObject HUD_objects;

    public void SetHudActiveState(bool active)
    {
        HUD_objects.SetActive(active);
    }
    public void SetCursorVisibleState(bool value)
    {
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = value;
    }


    private void Awake()
    {
        PauseControlSystem pauseControlSystem = FindFirstObjectByType<PauseControlSystem>();

        pauseControlSystem.CursorModeChanged += SetCursorVisibleState;
        pauseControlSystem.HudActiveChanged += SetHudActiveState;
    }
}
