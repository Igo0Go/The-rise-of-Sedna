using UnityEngine;
using System;

public class EnemyPart : EnemyBase
{
    [SerializeField, Min(1)]
    private float deadTime = 3;

    [Min(1)]
    public int destroyDamageForMain = 1;

    public event Action<EnemyPart> OnDestroyPart;

    protected override void Dead()
    {
        OnDestroyPart?.Invoke(this);
        gameObject.AddComponent<Rigidbody>();
        Destroy(gameObject, deadTime);
    }
}
