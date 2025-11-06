using UnityEngine;
using System;

public class CollectingQuestTargetModule : InteractiveModule
{
    [SerializeField, Min(0)]
    private int questId = 0;

    public event Action<int> TryCompleteSearchQuest;


    public override void Activate()
    {
        base.Activate();
        TryCompleteSearchQuest.Invoke(questId);
    }
}
