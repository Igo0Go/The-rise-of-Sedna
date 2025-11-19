using UnityEngine;
using UnityEngine.Events;

public abstract class BaseAlly : MonoBehaviour
{
    [Min(1)]
    public int ID = 1;
    [SerializeField, Min(1)]
    protected int HP;

    public UnityEvent<BaseAlly> deadEvent;

    public virtual void GetDamage(int damage)
    {
        if (HP <= 0)
            return;

        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            deadEvent.Invoke(this);
            Dead();
        }
    }
    protected abstract void Dead();
}
