using System.Linq;
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

        QuestStateModule[] questModules = 
           FindObjectsByType<QuestStateModule>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (QuestStateModule module in questModules)
        {
            module.QuestStateChanged += OnTryQuestStateChange;
        }

        ActivationQuestTarget[] actiovationQuestTargets =
    FindObjectsByType<ActivationQuestTarget>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (ActivationQuestTarget target in actiovationQuestTargets)
        {
            target.targetActivated += OnActivateQuestTarget;
        }

        jornal = FindFirstObjectByType<QuestJornal>();

        if (jornal != null)
        {
            jornal.SelectedQuestStateCategoryChanged += FindQuestsForJornalByState;
            jornal.SelectedQuestChanged += FindQuestForJornalById;
        }

        QuestDetailModule[] questDetailsModules =
    FindObjectsByType<QuestDetailModule>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (QuestDetailModule module in questDetailsModules)
        {
            module.QuestDetailChanged += OnQuestDetailUnblock;
        }

        questSystem.newInfoInJornal += OnNewInfoInJornal;

        #endregion

        CollectingQuestTargetModule[] searchQuestTargetModules =
    FindObjectsByType<CollectingQuestTargetModule>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (CollectingQuestTargetModule module in searchQuestTargetModules)
        {
            module.TryCompleteSearchQuest += OnTryCompleteSearchQuest;
        }

        EnemyBase[] enemies =
            FindObjectsByType<EnemyBase>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        questSystem.SubscribeQuestsToEnemyDeadEvent(enemies.ToList());
    }

    #region Старый код

    private void FindQuestsForJornalByState(QuestState state)
    {
        jornal.UpdateAllQuestButtons(questSystem.GetAllQuestsWithState(state));
    }

    private void FindQuestForJornalById(int id)
    {
        QuestBase quest = questSystem.GetQuestById(id);
        quest.dirty = false;
        jornal.DrawQuestData(quest);
    }

    private void OnActivateQuestTarget(ActivationQuestTarget target)
    {
        questSystem.ActivationQuestTargetUsed(target.ID);
    }

    private void OnTryQuestStateChange(int id, QuestState state)
    {
        questSystem.SetStateForQuestById(id, state);
    }

    private void OnQuestDetailUnblock(int questId, int detailIndex)
    {
        questSystem.UnblockQuestDetails(questId, detailIndex);
    }

    private void OnNewInfoInJornal()
    {
        LogPanel.instance.ShowStringInLog("Новая запись в журнале");
    }

    private void OnTryCompleteSearchQuest(int id)
    {
        questSystem.TryCompleteCollectingQuest(id);
    }
    #endregion
}
