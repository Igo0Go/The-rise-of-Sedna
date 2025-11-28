using UnityEngine;

public class MedPack : InteractiveObject
{
    [SerializeField]
    private MedItemData data;

    public override (string name, string action) GetData()
    {
        return (data.itemName, data.actionDescription + "\nВосстановить " + data.hpValue + " ОЗ");
    }

    public override void Use()
    {
        InventarySystem.Instance.AddToInventory(data.id, 1);
        Destroy(gameObject);
    }
}
