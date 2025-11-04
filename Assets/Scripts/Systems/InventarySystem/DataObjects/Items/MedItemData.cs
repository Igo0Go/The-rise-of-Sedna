using UnityEngine;

[CreateAssetMenu(fileName = "MedItemData", menuName = "IgoGoTools/Items/MedItemData")]
public class MedItemData : InventoryItemData
{
    [Min(1)]
    public int hpValue = 10;
}
