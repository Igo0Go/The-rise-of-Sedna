using UnityEngine;
using System;

public class QuestStateModule : MonoBehaviour
{
    [SerializeField]
    [Min(0)]
    private int targetQuestID;
    [SerializeField]
    private QuestState targetState;

    public event Action<int, QuestState> QuestStateChanged;

    public void Activate()
    {
        QuestStateChanged?.Invoke(targetQuestID, targetState);
    }
}
