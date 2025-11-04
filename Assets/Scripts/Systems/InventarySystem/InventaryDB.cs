using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "InventaryDB", menuName = "IgoGo_Tools/InventaryDB")]
public class InventaryDB : ScriptableObject
{
    [SerializeField]
    private List<InventoryItemData> items;

    public InventoryItemData FindById(int id)
    {
        InventoryItemData item = items.Find(x => x.id == id);
        return item;
    }
}
