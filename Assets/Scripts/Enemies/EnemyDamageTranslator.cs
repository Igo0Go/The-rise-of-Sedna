using UnityEngine;

public class EnemyDamageTranslator : EnemyBase
{
    [SerializeField]
    private EnemyBase target;

    public override void GetDamage(int damage)
    {
        target.GetDamage(damage);
    }

    protected override void Dead()
    {
        HP = 100;
    }
}
