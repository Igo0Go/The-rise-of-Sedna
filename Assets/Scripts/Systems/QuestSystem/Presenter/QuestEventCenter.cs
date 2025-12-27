using System.Linq;
using UnityEngine;

public class QuestEventCenter : MonoBehaviour
{
    [SerializeField]
    private TextAsset _textAsset;

    private QuestSystem questSystem;
    private QuestJournal journal;

    void Awake()
    {
        #region Развёртывание системы квестов

        questSystem = new QuestSystem(_textAsset.text);
        questSystem.NewInfo += OnNewInfo;

        #endregion

        #region Организация связей с модулями

        #region Модуль изменения состояния квеста

        QuestStateModule[] questModules =
            FindObjectsByType<QuestStateModule>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (QuestStateModule module in questModules)
        {
            module.QuestStateChanged += OnTryQuestStateChange;
        }

        #endregion

        #region Модуль отслеживания состояния квеста

        QuestStateObserver[] questObservers =
            FindObjectsByType<QuestStateObserver>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (QuestStateObserver observer in questObservers)
        {
            questSystem.QuestStateChanged += observer.OnQuestState;
        }

        #endregion

        #region Модуль разблокировки деталей квеста

        QuestDetailModule[] questDetailsModules =
            FindObjectsByType<QuestDetailModule>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (QuestDetailModule module in questDetailsModules)
        {
            module.QuestDetailChanged += OnQuestDetailUnblock;
        }

        #endregion

        #region Цель квеста типа "Активация"

        ActivationQuestTarget[] actiovationQuestTargets =
            FindObjectsByType<ActivationQuestTarget>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (ActivationQuestTarget target in actiovationQuestTargets)
        {
            target.targetActivated += OnActivateQuestTarget;
        }

        #endregion

        #region Цель квеста типа "Сбор"

        CollectingQuestTargetModule[] searchQuestTargetModules =
            FindObjectsByType<CollectingQuestTargetModule>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (CollectingQuestTargetModule module in searchQuestTargetModules)
        {
            module.TryCompleteSearchQuest += OnTryCompleteSearchQuest;
        }

        #endregion

        #region Цель квеста типа "Охота"

        EnemyBase[] enemies = FindObjectsByType<EnemyBase>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        questSystem.SubscribeQuestsToEnemyDeadEvent(enemies.ToList());

        #endregion

        #endregion

        #region Связь с журналом

        journal = FindFirstObjectByType<QuestJournal>();
        if (journal != null)
        {
            journal.SelectedQuestStateCategoryChanged += FindQuestsForJornalByState;
            journal.SelectedQuestChanged += FindQuestForJornalById;
        }

        #endregion
    }

    #region Связь с журналом

    private void FindQuestsForJornalByState(QuestState state)
    {
        journal.UpdateAllQuestButtons(questSystem.GetAllQuestsWithState(state));
    }

    private void FindQuestForJornalById(int id)
    {
        QuestBase quest = questSystem.GetQuestById(id);
        quest.containceNewInfo = false;
        journal.DrawQuestData(quest);
    }

    #endregion

    #region Управление квестами

    private void OnTryQuestStateChange(int id, QuestState state)
    {
        questSystem.SetStateForQuestById(id, state);
    }
    private void OnQuestDetailUnblock(int questId, int detailIndex)
    {
        questSystem.UnblockQuestDetails(questId, detailIndex);
    }

    #endregion

    #region Квесты по типам

    private void OnActivateQuestTarget(ActivationQuestTarget target)
    {
        questSystem.ActivationQuestTargetUsed(target.ID);
    }

    private void OnTryCompleteSearchQuest(int id)
    {
        questSystem.TryCompleteCollectingQuest(id);
    }

    #endregion

    private void OnNewInfo()
    {
        LogPanel.instance.ShowStringInLog("Новая запись в журнале");
    }
}
