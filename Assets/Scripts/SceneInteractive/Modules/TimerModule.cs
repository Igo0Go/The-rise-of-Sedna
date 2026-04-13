using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TimerModule : InteractiveModule
{
    [SerializeField, Min(0)]
    private float time = 1;
    [SerializeField]
    private bool playOnStart = false;
    [SerializeField]
    private UnityEvent actionAfterTime;

    [SerializeField]
    private bool useDebug = false;

    private float currentTime;

    private void Start()
    {
        currentTime = time;
        if (playOnStart)
        {
            Activate();
        }
    }

    public override void Activate()
    {
        IsActive = true;
        StartCoroutine(TimerCoroutine());
    }

    public override void Deactivate()
    {
        IsActive = false;
        StopAllCoroutines();
    }

    public override void ToDefaultState()
    {
        Deactivate();
        currentTime = time;
    }

    private IEnumerator TimerCoroutine()
    {
        while(currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            yield return null;
        }
        actionAfterTime.Invoke();
    }

    private void OnDrawGizmos()
    {
        if(useDebug)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(transform.position, 0.3f);

            int count = actionAfterTime.GetPersistentEventCount();

            for (int i = 0; i < count; i++)
            {
                Transform target = actionAfterTime.GetPersistentTarget(i).GameObject().transform;
                Gizmos.DrawLine(transform.position, target.position);
            }
        }
    }
}
