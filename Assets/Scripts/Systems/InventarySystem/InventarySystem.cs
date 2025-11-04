using System.Collections.Generic;
using System;

public class InventarySystem
{
    private InventaryDB db;

    private InventarySystem() { }
    public static InventarySystem Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new InventarySystem();
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }
    private static InventarySystem _instance = null;

    public void SetDb(InventaryDB dbInstance)
    {
        db = dbInstance;
    }

    public static void Clear() => Instance = null;

    private Dictionary<int, int> inventory = new();

    public event Action<int, int> InventoryChanged;

    public void AddToInventory(int itemId, int count)
    {
        if (inventory.ContainsKey(itemId))
        {
            inventory[itemId]+= count;
            InventoryChanged?.Invoke(itemId, inventory[itemId]);
        }
        else
        {
            inventory.Add(itemId, count);
            InventoryChanged?.Invoke(itemId, inventory[itemId]);
        }
    }

    public bool TrySpendItem(int itemId, int count)
    {
        if (!inventory.ContainsKey(itemId)) 
            return false;

        if (inventory[itemId] >= count)
        {
            inventory[itemId] -= count;
            if (inventory[itemId] <= 0)
            {
                inventory.Remove(itemId);
                InventoryChanged?.Invoke(itemId, 0);
            }
            else
            {
                InventoryChanged?.Invoke(itemId, inventory[itemId]);
            }
            return true;
        }

        return false;
    }

    public (InventoryItemData obj, int count) GetInventoryItemDataById(int itemId)
    {
        InventoryItemData obj = db.FindById(itemId);

        if (obj != null)
        {
            if(inventory.ContainsKey(itemId))
            {
                return (obj, inventory[itemId]);
            }
        }

        return (null, 0);
    }
}
