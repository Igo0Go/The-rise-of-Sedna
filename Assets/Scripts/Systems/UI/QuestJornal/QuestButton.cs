using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestButton : MonoBehaviour
{
    [SerializeField]
    private TMP_Text questNameText;
    [SerializeField]
    private Button button;
    [SerializeField]
    private GameObject dirtyObject;

    public event Action<int> OnChooseQuest;

    private int savedId;

    public void Init(QuestBase questData)
    {
        questNameText.text = questData.name;
        savedId = questData.id;

        dirtyObject.SetActive(questData.containceNewInfo);

        button.onClick.AddListener(OnClickToButton);
    }

    public void OnClickToButton()
    {
        dirtyObject.SetActive(false);
        button.image.color = Color.white;
        OnChooseQuest?.Invoke(savedId);
    }
}
