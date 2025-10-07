using TMPro;
using UnityEngine;

public class ObjectiveSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private TMP_Text objectiveText;

    private void Awake()
    {
        panel.SetActive(false);
    }

    public void ShowNewObjective(ObjectivePack pack)
    {
        panel.SetActive(true);
        objectiveText.text = pack.taskString.ToString();
    }

    public void ClosePanel() => panel.SetActive(false);
}
