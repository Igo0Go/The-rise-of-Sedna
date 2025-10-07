using UnityEngine;
using UnityEngine.Events;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField, Min(1)]
    protected int HP;

    public UnityEvent<EnemyBase> deadEvent;

    public virtual void GedDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Dead();
            deadEvent.Invoke(this);
        }
    }
    protected abstract void Dead();
}
