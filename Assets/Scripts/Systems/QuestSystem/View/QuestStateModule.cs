using UnityEngine;
using System;

public class QuestStateModule : MonoBehaviour
{
    [SerializeField]
    [Min(0)]
    private int targetQuestID;
    [SerializeField]
    private QuestState targetState;

    public event Action<int, QuestState> OnQuestStateChange;

    public void Activate()
    {
        OnQuestStateChange?.Invoke(targetQuestID, targetState);
    }
}
