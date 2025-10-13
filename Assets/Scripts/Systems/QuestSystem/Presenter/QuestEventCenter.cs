using UnityEngine;

public class QuestEventCenter : MonoBehaviour
{
    [SerializeField]
    private TextAsset _textAsset;

    QuestSystem questSystem;


    [Header("Тест")]
    [SerializeField, Min(0)]
    private int targetQuestId = 0;
    [SerializeField]
    private QuestState targetQuestState = QuestState.faled;


    void Awake()
    {
        questSystem = new QuestSystem(_textAsset.text);
        questSystem.questStateChanged += OnQuestStateChanged;
    }

    private void OnQuestStateChanged(QuestBase quest)
    {
        Debug.Log(quest.name + " теперь " + quest.state);
    }

    [ContextMenu("Тест")]
    public void QuestStateTest()
    {
        questSystem.SetStateForQuestById(targetQuestId, targetQuestState);
    }
}
