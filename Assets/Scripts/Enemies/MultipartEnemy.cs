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
        gameObject.AddComponent<Rigidbody>();
        Destroy(deadTarget, deadTime);
    }
}
