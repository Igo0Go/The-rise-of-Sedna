using System;
using UnityEngine;

public class QuestDetailModule : MonoBehaviour
{
    [SerializeField, Min(0)]
    private int targetQuestID;
    [SerializeField, Min(0)]
    private int detailIndexForUnblock;


    public event Action<int, int> QuestDetailChanged;

    public void Activate()
    {
        QuestDetailChanged?.Invoke(targetQuestID, detailIndexForUnblock);
    }
}
