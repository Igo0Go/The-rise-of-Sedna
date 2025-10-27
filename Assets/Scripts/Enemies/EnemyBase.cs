using UnityEngine;
using UnityEngine.Events;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField, Min(1)]
    protected int HP;

    public UnityEvent<EnemyBase> deadEvent;

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
