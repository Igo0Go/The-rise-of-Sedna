using UnityEngine;
using System;

public class SearchQuestTargetModule : InteractiveModule
{
    [SerializeField, Min(0)]
    private int questId = 0;

    public event Action<int, FPC_InventorySystem> TryCompleteSearchQuest;


    public override void Activate()
    {
        base.Activate();

        FPC_InventorySystem inventorySystem = FindFirstObjectByType<FPC_InventorySystem>();

        TryCompleteSearchQuest.Invoke(questId, inventorySystem);
    }
}
