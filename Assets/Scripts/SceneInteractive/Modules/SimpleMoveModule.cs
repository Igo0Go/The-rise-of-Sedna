using System.Collections;
using UnityEngine;

public class SimpleMoveModule : InteractiveModule
{
    [SerializeField]
    private Transform startPoint;
    [SerializeField]
    private Transform endPoint;
    [SerializeField]
    private AnimationCurve movingCurve;
    [SerializeField, Min(0.01f)]
    private float moveTime = 1;
    [SerializeField]
    private Transform moveObject;

    public override void Activate()
    {
        base.Activate();
        StopAllCoroutines();
        StartCoroutine(MoveCoroutine(endPoint));
    }
    public override void Deactivate()
    {
        base.Deactivate();
        StopAllCoroutines();
        StartCoroutine(MoveCoroutine(startPoint));
    }
    public override void ToDefaultState()
    {
        base.ToDefaultState();
        Deactivate();
    }

    private IEnumerator MoveCoroutine(Transform target)
    {
        Vector3 startPoint = moveObject.position;

        float t = 0;
        float progress = 0;
        while(t < 1)
        {
            t += Time.deltaTime / moveTime;
            progress = movingCurve.Evaluate(t);
            moveObject.position = Vector3.Lerp(startPoint, target.position, progress);
            yield return null;
        }
        moveObject.position = target.position;
    }
}
