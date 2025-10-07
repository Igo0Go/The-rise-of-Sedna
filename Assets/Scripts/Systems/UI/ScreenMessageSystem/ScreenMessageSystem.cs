using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMessageSystem : MonoBehaviour
{
    [SerializeField]
    private PauseControlSystem pausedControlSystem;
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private TMP_Text messageNameText;
    [SerializeField]
    private TMP_Text messageText;
    [SerializeField]
    private Button continueButton;

    private void Awake()
    {
        panel.SetActive(false);
        continueButton.onClick.AddListener(OnContinue);
    }

    public void ShowNewMessage(ScreenMessagePack pack)
    {
        pausedControlSystem.IsMessage = true;
        panel.SetActive(true);
        messageNameText.text = pack.messageName;
        messageText.text = pack.messageText;
    }

    public void OnContinue()
    {
        pausedControlSystem.IsMessage = false;
        panel.SetActive(false);
    }

}
