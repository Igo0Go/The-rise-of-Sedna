using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class QuestJornal : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private GameObject questButtonPrefab;
    [SerializeField]
    private Transform questButtonsContainer;
    [SerializeField]
    private TMP_Text selectedQuestName;
    [SerializeField]
    private TMP_Text selectedQuestDescription;

    private PauseControlSystem pauseSystem;

    public event Action<int> SelectedQuestChanged;
    public event Action<QuestState> SelectedQuestStateCategoryChanged;

    private void Awake()
    {
        ClearAll();
        pauseSystem = FindFirstObjectByType<PauseControlSystem>();
    }

    public void ClearAll()
    {
        ClearSelectedQuestView();
        ClearSelectedQuestStateCategory();
    }

    public void JornalToggle()
    {
        if(panel.activeSelf)
        {
            ClosePanel();
        }
        else
        {
            OpenPanel();
        }
    }
    public void OpenPanel()
    {
        pauseSystem.IsJornal = true;
        panel.SetActive(true);
        ClearAll();
    }
    public void ClosePanel()
    {
        pauseSystem.IsJornal = false;
        panel.SetActive(false);
    }

    private void ClearSelectedQuestView()
    {
        selectedQuestName.text = string.Empty;
        selectedQuestDescription.text = string.Empty;
    }
    private void ClearSelectedQuestStateCategory()
    {
        for (int i = questButtonsContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(questButtonsContainer.GetChild(i).gameObject);
        }
    }

    public void ShowActiveQuests()
    {
        SelectedQuestStateCategoryChanged?.Invoke(QuestState.active);
    }
    public void ShowCompleteQuests()
    {
        SelectedQuestStateCategoryChanged?.Invoke(QuestState.complete);
    }
    public void ShowFaledQuests()
    {
        SelectedQuestStateCategoryChanged?.Invoke(QuestState.faled);
    }

    public void UpdateAllQuestButtons(List<QuestBase> quests)
    {
        ClearSelectedQuestStateCategory();
        ClearSelectedQuestView();

        foreach (QuestBase quest in quests)
        {
            QuestButton button = Instantiate(questButtonPrefab, questButtonsContainer).
                GetComponent<QuestButton>();
            button.Init(quest);
            button.OnChooseQuest += OnQuestSelect;
        }
    }
    private void OnQuestSelect(int id)
    {
        SelectedQuestChanged?.Invoke(id);
    }
    public void DrawQuestData(QuestBase quest)
    {
        selectedQuestName.text = quest.name;
        selectedQuestDescription.text = quest.description;
    }
}
