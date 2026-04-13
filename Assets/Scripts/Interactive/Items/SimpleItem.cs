using UnityEngine;

public class SimpleItem : InteractiveObject
{
    [SerializeField]
    private InventoryItemData data;

    public override (string name, string action) GetData()
    {
        return (data.itemName, data.actionDescription);
    }

    public override void Use()
    {
        onUseEvent?.Invoke();
        InventarySystem.Instance.AddToInventory(data.id, 1);
        Destroy(gameObject);
    }
}
