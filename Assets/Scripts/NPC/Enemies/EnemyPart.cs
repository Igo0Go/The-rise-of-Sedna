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
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        float x = UnityEngine.Random.Range(-1, 1);
        float y = UnityEngine.Random.Range(-1, 1);
        float z = UnityEngine.Random.Range(-1, 1);
        Vector3 vector = new Vector3(x, y, z);
        rb.AddForce(vector * 10, ForceMode.Impulse);
        base.Dead();
        Destroy(gameObject, deadTime);
    }
}
