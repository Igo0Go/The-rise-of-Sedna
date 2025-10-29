using System;
using TMPro;
using UnityEngine;

public class QuestButton : MonoBehaviour
{
    [SerializeField]
    private TMP_Text questNameText;

    public event Action<int> OnChooseQuest;

    private int savedId;

    public void Init(QuestBase questData)
    {
        questNameText.text = questData.name;
        savedId = questData.id;
    }

    public void OnClickToButton()
    {
        OnChooseQuest?.Invoke(savedId);
    }
}
