using UnityEngine;
using UnityEngine.Events;

public class QuestStateObserver : MonoBehaviour
{
    [SerializeField]
    [Min(0)]
    private int targetQuestId = 0;
    [SerializeField]
    private QuestState targetState;
    [SerializeField]
    private UnityEvent actions;

    public void OnQuestState(QuestBase quest)
    {
        if(quest.id == targetQuestId && targetState == quest.State)
        {
            actions.Invoke();
        }
    }
}
