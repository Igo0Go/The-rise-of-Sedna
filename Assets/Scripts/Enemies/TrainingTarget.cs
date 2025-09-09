using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TrainingTarget : EnemyBase
{
    [SerializeField, Min(1)]
    private int maxHP = 100;
    [SerializeField]
    private GameObject targetObjs;

    protected override void Dead()
    {
        StartCoroutine(DeadCoroutine());
    }

    private IEnumerator DeadCoroutine()
    {
        targetObjs.SetActive(false);
        Collider col = GetComponent<Collider>();
        col.enabled = false;
        yield return new WaitForSeconds(3);
        col.enabled = true;
        targetObjs.SetActive(true);
        HP = 100;
    }
}
