using UnityEngine;
using System.Collections.Generic;
using System;

public class FPC_InventorySystem : MonoBehaviour
{
    private Dictionary<InventoryObject, int> inventory = new();

    public event Action<InventoryObject, int> InventoryChanged;

    public void AddToInventory(InventoryObject obj)
    {
        if(inventory.ContainsKey(obj))
        {
            inventory[obj]++;
            InventoryChanged?.Invoke(obj, inventory[obj]);
        }
        else
        {
            inventory.Add(obj, 1);
            InventoryChanged?.Invoke(obj, inventory[obj]);
        }
    }

    public void RemoveFromInventory(int objId)
    {
        InventoryObject bufer = FindById(objId);

        if(bufer != null)
        {
            if (inventory[bufer] > 1)
            {
                inventory[bufer]--;
                InventoryChanged?.Invoke(bufer, inventory[bufer]);
            }
            else
            {
                inventory.Remove(bufer);
                InventoryChanged?.Invoke(bufer, 0);
            }
        }
    }

    public bool TrySpendItem(int Id, int count)
    {
        InventoryObject bufer = FindById(Id);

        if (inventory[bufer] >= count)
        {
            inventory[bufer] -= count;
            if (inventory[bufer] <= 0)
            {
                inventory.Remove(bufer);
                InventoryChanged?.Invoke(bufer, 0);
            }
            else
            {
                InventoryChanged?.Invoke(bufer, inventory[bufer]);
            }
            return true;
        }

        return false;
    }

    public (InventoryObject obj, int count) GetInventoryItemDataById(int id)
    {
        InventoryObject obj = FindById(id);

        if(obj != null)
        {
            return (obj, inventory[obj]);
        }

        return (null, 0);
    }

    public InventoryObject FindById(int id)
    {
        InventoryObject bufer = null;
        foreach (InventoryObject obj in inventory.Keys)
        {
            if (obj.id == id)
            {
                bufer = obj;
                break;
            }
        }
        return bufer;
    }
}
