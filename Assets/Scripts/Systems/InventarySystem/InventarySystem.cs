using System.Collections.Generic;
using System;
using System.Linq;

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

    private Dictionary<int, int> inventory = new();
    public List<WeaponMagazine> magazines = new();

    public event Action<int, int> InventoryChanged;
    public event Action MagazinesChanged;

    public void SetDb(InventaryDB dbInstance)
    {
        db = dbInstance;
    }

    public static void Clear() => Instance = null;

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

        InventoryItemData data = db.FindById(itemId);
        if (data is MagazineData dataMagazine)
        {
            for (int i = 0; i < count; i++)
            {
                magazines.Add(dataMagazine.GetMagazine());
            }
            MagazinesChanged?.Invoke();
        }

        LogPanel.instance.ShowStringInLog("Добавлено: " + data.name + "(" + count + ")");
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

            InventoryItemData data = db.FindById(itemId);

            int removed = 0;

            if (data is MagazineData dataMagazine)
            {
                for (int i = magazines.Count-1; i >= 0; i--)
                {
                    if (magazines[i].data.id == itemId)
                    {
                        magazines.RemoveAt(i);
                        removed++;
                        if(removed >= count)
                        {
                            break;
                        }
                    }
                }
                MagazinesChanged?.Invoke();
            }
            LogPanel.instance.ShowStringInLog("Удалено: " + data.name + "(" + count + ")");

            return true;
        }

        return false;
    }

    public void AddConcreteMagazine(WeaponMagazine weaponMagazine)
    {
        magazines.Add(weaponMagazine);

        int itemId = weaponMagazine.data.id;
        if (inventory.ContainsKey(itemId))
        {
            inventory[itemId] ++;
            InventoryChanged?.Invoke(itemId, inventory[itemId]);
        }
        else
        {
            inventory.Add(itemId, 1);
            InventoryChanged?.Invoke(itemId, inventory[itemId]);
        }

        MagazinesChanged?.Invoke();
    }
    public void RemoveConcreteMagazine(WeaponMagazine weaponMagazine)
    {
        if(magazines.Contains(weaponMagazine))
        {
            magazines.Remove(weaponMagazine);
            MagazinesChanged?.Invoke();
        }

        int itemId = weaponMagazine.data.id;

        inventory[itemId] --;
        if (inventory[itemId] <= 0)
        {
            inventory.Remove(itemId);
            InventoryChanged?.Invoke(itemId, 0);
        }
        else
        {
            InventoryChanged?.Invoke(itemId, inventory[itemId]);
        }
    }

    public List<WeaponMagazine> GetMagazinesOfType(MagazineType magazineType)
    {
        return magazines.Where(m => m.data.type == magazineType).ToList<WeaponMagazine>();
    }

    public (InventoryItemData item, int count) GetInventoryItemDataById(int itemId)
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
