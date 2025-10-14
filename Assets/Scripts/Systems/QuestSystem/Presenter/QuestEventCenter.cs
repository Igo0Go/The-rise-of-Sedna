using UnityEngine;

public class QuestEventCenter : MonoBehaviour
{
    [SerializeField]
    private TextAsset _textAsset;

    QuestSystem questSystem;

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
        #endregion 

        ActivationQuestTarget[] actiovationQuestTargets =
   FindObjectsByType<ActivationQuestTarget>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (ActivationQuestTarget target in actiovationQuestTargets)
        {
            target.targetActivated += OnActivateQuestTarget;
        }
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
