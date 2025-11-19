using UnityEngine;
using System.Collections.Generic;

public class MultipartEnemy : EnemyBase
{
    [SerializeField]
    private List<EnemyPart> enemyParts;
    [SerializeField, Min(1)]
    private float deadTime = 3;
    [SerializeField]
    private GameObject deadTarget;

    private void Awake()
    {
        foreach (EnemyPart part in enemyParts)
        {
            part.OnDestroyPart += OnPartDestroy;
        }
    }

    private void OnPartDestroy(EnemyPart part)
    {
        GetDamage(part.destroyDamageForMain);
    }

    protected override void Dead()
    {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        float x = Random.Range(-1, 1);
        float y = Random.Range(-1, 1);
        float z = Random.Range(-1, 1);
        Vector3 vector = new Vector3(x, y, z);
        rb.AddForce(vector * 10, ForceMode.Impulse);
        base.Dead();
        Destroy(deadTarget, deadTime);
    }
}
