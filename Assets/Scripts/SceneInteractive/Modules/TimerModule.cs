using System.Collections;
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
}
