using UnityEngine;
using UnityEngine.Events;

public abstract class EnemyBase : MonoBehaviour
{
    [Min(1)]
    public int ID = 1;
    [SerializeField, Min(1)]
    protected int HP;
    [SerializeField, Min(1)]
    protected int Exp = 1;

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
    protected virtual void Dead()
    {
        SkillHolder.Instance.AddExperience(Exp);
    }
}
