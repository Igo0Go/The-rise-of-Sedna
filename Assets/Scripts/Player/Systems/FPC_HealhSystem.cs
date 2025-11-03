using System;
using UnityEngine;

public class FPC_HealhSystem : MonoBehaviour
{
    [SerializeField, Min(1)]
    private int maxHealth = 100;

    public int Health
    {
        get
        {
            return _health;
        }
        private set
        {
            _health = Math.Clamp(value, 0, maxHealth);
            OnHealthStateChanged?.Invoke(_health, maxHealth);

            if (_health <= 0)
            {
                Dead();
            }
        }
    }
    private int _health;

    public event Action OnDead;
    public event Action OnDamage;
    public event Action<int, int> OnHealthStateChanged;
    public event Action<int> MedPackCountChanged;

    private void Awake()
    {
        FindFirstObjectByType<FPC_InventorySystem>().InventoryChanged += OnIventoryChanged;
    }

    private void Start()
    {
        Health = maxHealth;
    }

    public void Dead()
    {
        OnDead?.Invoke();
    }

    public void GetDamage(int damage)
    {
        if(Health > 0)
        {
            Health -= damage;
            OnDamage?.Invoke();
        }
    }

    public bool Heal(int heal)
    {
        if(Health == maxHealth)
        {
            return false;
        }

        Health += heal;
        return true;
    }

    public void UseMedPack()
    {
        if(Health >= maxHealth)
        {
            return;
        }

        FPC_InventorySystem inventorySystem = FindFirstObjectByType<FPC_InventorySystem>();

        (InventoryObject data, int count) info = inventorySystem.
            GetInventoryItemDataById(IdHolder.InventoryItemsIds.Biogel);

        if(info.count > 0)
        {
            MedItemData medData = info.data as MedItemData;
            Heal(medData.hpValue);
            inventorySystem.RemoveFromInventory(IdHolder.InventoryItemsIds.Biogel);
        }
    }

    public void OnIventoryChanged(InventoryObject obj, int count)
    {
        if(obj.id == IdHolder.InventoryItemsIds.Biogel)
        {
            MedPackCountChanged?.Invoke(count);
        }
    }
}
