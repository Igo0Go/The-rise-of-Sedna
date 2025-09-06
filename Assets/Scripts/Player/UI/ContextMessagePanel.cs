using TMPro;
using UnityEngine;

public class ContextMessagePanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text itemNameText;
    [SerializeField]
    private TMP_Text actionDescriptionText;

    private void Awake()
    {
        FPC_Interaction fPC_Interaction = FindFirstObjectByType<FPC_Interaction>();
        fPC_Interaction.TakeInteractiveObject += OnCLearContext;
        fPC_Interaction.LostInteractiveObject += OnCLearContext;
        fPC_Interaction.NewInteractiveObjectFound += OnChengeContext;
        OnCLearContext();
    }

    private void OnChengeContext(string itemName, string actionDescription)
    {
        itemNameText.gameObject.SetActive(true);
        actionDescriptionText.gameObject.SetActive(true);
        itemNameText.text = itemName;
        actionDescriptionText.text = actionDescription;

    }
    private void OnCLearContext()
    {
        itemNameText.gameObject.SetActive(false);
        actionDescriptionText.gameObject.SetActive(false);
    }
}
