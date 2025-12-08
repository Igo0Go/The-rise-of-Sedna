using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FPC_DeadHUD : MonoBehaviour
{
    [SerializeField]
    private GameObject deadPanel;
    [SerializeField]
    private TMP_Text messageText;

    void Awake()
    {
        deadPanel.SetActive(false);
        FindFirstObjectByType<FPC_HealhSystem>().OnDead += OnDead;
    }

    public void ShowDefeadMessage(ScreenMessagePack pack)
    {
        messageText.text = pack.messageText;
        OnDead();
    }

    private void OnDead()
    {
        FindFirstObjectByType<PauseControlSystem>().IsDead = true;
        deadPanel.SetActive(true);
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
