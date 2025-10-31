using UnityEngine;

public class QuestEventCenter : MonoBehaviour
{
    [SerializeField]
    private TextAsset _textAsset;

    private QuestSystem questSystem;
    private QuestJornal jornal;

    void Awake()
    {
        #region предыдущий код

        questSystem = new QuestSystem(_textAsset.text);
        questSystem.questStateChanged += OnQuestStateChanged;

        QuestStateModule[] questModules = 
           FindObjectsByType<QuestStateModule>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (QuestStateModule module in questModules)
        {
            module.OnQuestStateChange += OnTryQuestStateChange;
        }

        ActivationQuestTarget[] actiovationQuestTargets =
    FindObjectsByType<ActivationQuestTarget>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (ActivationQuestTarget target in actiovationQuestTargets)
        {
            target.targetActivated += OnActivateQuestTarget;
        }

        #endregion 

        jornal = FindFirstObjectByType<QuestJornal>();

        if (jornal != null)
        {
            jornal.SelectedQuestStateCategoryChanged += FindQuestsForJornalByState;
            jornal.SelectedQuestChanged += FindQuestForJornalById;
        }
    }

    private void FindQuestsForJornalByState(QuestState state)
    {
        jornal.UpdateAllQuestButtons(questSystem.GetAllQuestsWithState(state));
    }

    private void FindQuestForJornalById(int id)
    {
        jornal.DrawQuestData(questSystem.GetQuestById(id));
    }

    private void OnActivateQuestTarget(ActivationQuestTarget target)
    {
        questSystem.ActivationQuestTargetUsed(target.ID);
    }

    private void OnTryQuestStateChange(int id, QuestState state)
    {
        questSystem.SetStateForQuestById(id, state);
    }

    private void OnQuestStateChanged(QuestBase quest)
    {
        Debug.Log(quest.name + " теперь " + quest.State);
    }
}
