using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class MechanicBot : BaseAlly
{
    [SerializeField]
    private AudioClip deadClip;


    [SerializeField]
    private StationState stationState;
    [SerializeField]
    private MoveToPointState moveToPointState;
    [SerializeField]
    private HideState hideState;
    [SerializeField]
    private WorkState workState;

    private Action currentStateLoop;
    private Action lastState;

    private Transform currentTarget;

    private void Start()
    {
        currentTarget = moveToPointState.GetNextPathPoint();
        ToStationState();
    }

    private void Update()
    {
        if(HP > 0 && stationState.IsActive)
        {
            currentStateLoop();
        }
    }

    protected override void Dead()
    {
        AudioPack.audioSystem.PlaySoundInPoint(deadClip, transform.position, 50);
        Destroy(moveToPointState.agent.gameObject);
    }
    public override void GetDamage(int damage)
    {
        if(hideState.IsActive || HP <= 0)
        {
            return;
        }

        ToHideState();
        base.GetDamage(damage);
    }

    private void ToStationState()
    {
        moveToPointState.StopMove();
        stationState.StayInStation(moveToPointState.agent.transform);
        stationState.SetActiveState(false);
        lastState = ToStationState;
    }

    public void ToMoveToWorkState()
    {
        stationState.SetActiveState(true);
        stationState.DisconnectStation(moveToPointState.agent.transform);
        moveToPointState.ActivateMoveToTarget(currentTarget.position);
        lastState = ToMoveToWorkState;
        currentStateLoop = MoveToWorkLoop;
    }
    private void MoveToWorkLoop()
    {
        Vector3 target = currentTarget.position;
        if(moveToPointState.NearWithTarget(target))
        {
            if(currentTarget == workState.targetActor.transform)
            {
                ToWorkState();
            }
            else if(moveToPointState.CanContinuePath())
            {
                currentTarget = moveToPointState.GetNextPathPoint();
                moveToPointState.ActivateMoveToTarget(currentTarget.position);
            }
            else
            {
                currentTarget = workState.targetActor.transform;
                moveToPointState.ActivateMoveToTarget(currentTarget.position);
            }
        }
        else
        {
            moveToPointState.CorrectTarget(target);
        }
    }

    private void ToMoveToStationState()
    {
        stationState.SetActiveState(true);
        moveToPointState.ActivateMoveToTarget(stationState.station.transform.position);
        lastState = ToMoveToStationState;
        currentStateLoop = MoveToStationLoop;
    }
    private void MoveToStationLoop()
    {
        Vector3 target = stationState.station.transform.position;
        if (moveToPointState.NearWithTarget(target))
        {
            ToStationState();
        }
        else
        {
            moveToPointState.CorrectTarget(target);
        }
    }

    private void ToWorkState()
    {
        moveToPointState.StopMove();
        workState.StayInWorkPoint(moveToPointState.agent.transform);
        workState.SetActiveForWorkState(true);
        lastState = ToWorkState;
        currentStateLoop = WorkStateLoop;
    }
    private void WorkStateLoop()
    {
        if(workState.Complete)
        {
            workState.SetActiveForWorkState(false);
            ToMoveToStationState();
        }
        else
        {
            workState.WorkStateLoop();
        }
    }

    private void ToHideState()
    {
        moveToPointState.StopMove();
        hideState.SetHideState(true);
        currentStateLoop = HideStateLoop;
    }
    private void HideStateLoop()
    {
        if(hideState.CanStopHideLoop(transform.position))
        {
            hideState.SetHideState(false);
            lastState();
        }
    }
}

[Serializable]
public class StationState
{
    public bool IsActive { get; private set; }

    [SerializeField]
    private Animator animator;
    public Transform station;

    public void SetActiveState(bool value)
    {
        IsActive = value;
        animator.SetBool("UseShield", !value);
    }
    public void StayInStation(Transform bot)
    {
        bot.transform.position = station.position;
        bot.transform.rotation = station.rotation;
        bot.transform.parent = station;
    }
    public void DisconnectStation(Transform bot)
    {
        bot.transform.parent = null;
    }
}
[Serializable]
public class MoveToPointState
{
    public List<Transform> pathPoints;
    public NavMeshAgent agent;
    [SerializeField, Min(0.1f)]
    private float moveSpeed = 1;

    private int currentPathPoint = -1;

    public Transform CurrentPathTarget => pathPoints[currentPathPoint];
    public bool CanContinuePath()
    {
        return currentPathPoint <= pathPoints.Count - 2;
    }
    public Transform GetNextPathPoint()
    {
        currentPathPoint++;
        return pathPoints[currentPathPoint];
    }

    public void StopMove()
    {
        if(agent != null)
        {
            agent.isStopped = true;
        }
    }
    public void ActivateMoveToTarget(Vector3 target)
    {
        agent.isStopped = false;
        CorrectTarget(target);
    }
    public void CorrectTarget(Vector3 target)
    {
        agent.destination = target;
    }
    public bool NearWithTarget(Vector3 target)
    {
        float sqrDistance = Vector3.SqrMagnitude(target - agent.transform.position);
        if(sqrDistance < Mathf.Pow(moveSpeed*2, 2))
        {
            return true;
        }
        return false;
    }
}
[Serializable]
public class HideState
{
    public bool IsActive { get; private set; }

    [SerializeField]
    private Animator animator;
    [SerializeField, Min(1)]
    private float checkDistance = 1;
    [SerializeField, Min(1)]
    private float checkDelayTime = 1;
    [SerializeField]
    private LayerMask checkMask;

    private float currentCheckTime;

    public void SetHideState(bool hideState)
    {
        animator.SetBool("UseShield", hideState);
        IsActive = hideState;
    }
    public bool CanStopHideLoop(Vector3 center)
    {
        currentCheckTime += Time.deltaTime;
        if(currentCheckTime > checkDelayTime)
        {
            currentCheckTime = 0;

            Collider[] enemies = Physics.OverlapSphere(center, checkDistance, checkMask);

            if (enemies.Length <= 0)
            {
                return true;
            }
        }
        return false;
    }
}
[Serializable]
public class WorkState
{
    public bool Complete { get; private set; } = false;
    public Actor targetActor;

    [SerializeField]
    private Transform workPoint;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private int workPointsPerSecond;


    private float loopTime;
    private int currentCompleteWorkPoints;

    public void StayInWorkPoint(Transform bot)
    {
        bot.transform.position = workPoint.transform.position;
        bot.transform.rotation = workPoint.transform.rotation;
    }
    public void SetActiveForWorkState(bool workStateValue)
    {
        animator.SetBool("Work", workStateValue);
    }
    public void WorkStateLoop()
    {
        loopTime += Time.deltaTime;
        if(loopTime >= 1)
        {
            loopTime = 0;
            currentCompleteWorkPoints += workPointsPerSecond;

            if(currentCompleteWorkPoints >= targetActor.NpcWorkPoints)
            {
                targetActor.NPCAction();
                Complete = true;
            }
        }
    }
}
