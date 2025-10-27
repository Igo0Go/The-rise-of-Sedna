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

    public bool TryHeal(int heal)
    {
        if(Health == maxHealth)
        {
            return false;
        }

        Health += heal;
        return true;
    }
}
