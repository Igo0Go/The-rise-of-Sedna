using UnityEngine;
using System.Collections.Generic;
using System;

public class FPC_InventorySystem : MonoBehaviour
{
    private Dictionary<InventoryObject, int> inventory = new Dictionary<InventoryObject, int>();

    public event Action<InventoryObject, int> inventoryChanged;

    public void AddToInventory(InventoryObject obj)
    {
        if(inventory.ContainsKey(obj))
        {
            inventory[obj]++;
            inventoryChanged?.Invoke(obj, inventory[obj]);
        }
        else
        {
            inventory.Add(obj, 1);
            inventoryChanged?.Invoke(obj, inventory[obj]);
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
                inventoryChanged?.Invoke(bufer, inventory[bufer]);
            }
            else
            {
                inventory.Remove(bufer);
                inventoryChanged?.Invoke(bufer, 0);
            }
        }
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

    private InventoryObject FindById(int id)
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
