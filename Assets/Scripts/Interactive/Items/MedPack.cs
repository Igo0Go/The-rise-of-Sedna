using UnityEngine;

public class MedPack : InteractiveObject
{
    [SerializeField]
    private MedItemData data;

    public override (string name, string action) GetData()
    {
        return (data.name, data.actionDescription + "\nВосстановить " + data.hpValue + " ОЗ");
    }

    public override void Use()
    {
        FindFirstObjectByType<FPC_InventorySystem>().AddToInventory(data);
        Destroy(gameObject);
    }
}
