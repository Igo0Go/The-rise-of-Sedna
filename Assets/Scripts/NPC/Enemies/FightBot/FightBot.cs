using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FightBot : MultipartEnemy
{
    #region Fields
    [SerializeField]
    private Transform player;

    [Header("Dead")]
    [SerializeField]
    private GameObject deadExplosionPrefab;
    [SerializeField]
    private AudioClip deadClip;
    [SerializeField]
    private float hearingLevel = 0;

    [Space]

    [SerializeField]
    private MovePattern movePattern;
    [SerializeField]
    private ViewPattern viewPattern;
    [SerializeField]
    private SearchPattern searchPattern;
    [SerializeField]
    private AttackPattern attackPattern;
    [SerializeField]
    private ChoseTargetPattern choseTargetPattern;
    [SerializeField]
    private PatrollPattern patrollPattern;
    [SerializeField]
    private InteractionPattern interactionPattern;

    #endregion

    [SerializeField]
    private List<GameObject> markers;

    private Action currentLoop;
    private Vector3 currentTargetPoint;

    List<Transform> curentVisibleTargets = new List<Transform>();

    private void Start()
    {
        movePattern.agent.isStopped = true;

        foreach (BotTurret turret in attackPattern.turrets)
        {
            turret.OnDestroyPart += attackPattern.OnPartDestroy;
            turret.DamageEvent += OnGetDamage;
        }

        AudioPack.audioSystem.SoundEventInPoint += OnSoundEvent;
        ToPatrolState();
    }

    private void Update()
    {
        if (HP > 0)
        {
            currentLoop();
            interactionPattern.CheckManualInteractive(transform);
        }
    }

    private void DisableAllMarkers()
    {
        foreach (var marker in markers)
        {
            marker.SetActive(false);
        }
    }
    private void SetMarker(int index)
    {
        DisableAllMarkers();
        markers[index].gameObject.SetActive(true);
    }

    private void ToStayState()
    {
        if (HP <= 0)
            return;
        SetMarker(0);
        movePattern.StopMove();
        currentLoop = StayStateLoop;
    }
    private void StayStateLoop()
    {
        if(TargetsInView())
        {
            return;
        }

        if(patrollPattern.TimeToStateInPoint)
        {
            patrollPattern.StayInPointLoop();
        }
        else
        {
            ToPatrolState();
        }
    }

    private void ToPatrolState()
    {
        if (HP <= 0)
            return;
        SetMarker(1);
        if (patrollPattern.ContainsPatrolPoints())
        {
            currentTargetPoint = patrollPattern.GetTargetPatrolPoint().position;
            movePattern.ActivateMoveToTarget(currentTargetPoint);
            currentLoop = PatrolStateLoop;
        }
        else
        {
            ToStayState();
        }
    }
    private void PatrolStateLoop()
    {
        if (TargetsInView())
        {
            return;
        }

        if (movePattern.NearWithPoint(currentTargetPoint))
        {
            patrollPattern.AgentInPatrolPoint();
            ToStayState();
        }
        else
        {
            movePattern.CorrectTarget(currentTargetPoint);
        }
    }

    private void ToHuntingState()
    {
        if (HP <= 0)
            return;
        SetMarker(2);
        movePattern.ActivateMoveToTarget(currentTargetPoint);
        currentLoop = HuntingStateLoop;
    }
    private void HuntingStateLoop()
    {
        if(TargetsInView())
        {
            Transform currentTarget = choseTargetPattern.GetCurrentTarget(curentVisibleTargets);
            searchPattern.UpdateLastTarget(currentTarget);
            currentTargetPoint = currentTarget.position;
            movePattern.CorrectTarget(currentTargetPoint);

            if (attackPattern.TargetInAttackDistance(transform, currentTargetPoint))
            {
                movePattern.StopMove();
                attackPattern.UseWeapon(currentTargetPoint);
            }
            else
            {
                movePattern.ActivateMoveToTarget(currentTargetPoint);
            }
        }
        else
        {
            ToSearchState(searchPattern.GetLastTargetPoint());
        }
    }

    private void ToSearchState(Transform target)
    {
        if (HP <= 0)
            return;
        SetMarker(3);
        searchPattern.UpdateLastTarget(target);
        movePattern.ActivateMoveToTarget(searchPattern.GetLastTargetPoint());
        currentLoop = SearchStateLoop;
    }
    private void ToSearchState(Vector3 point)
    {
        if (HP <= 0)
            return;
        SetMarker(3);
        searchPattern.UpdateLastTargetPoint(point);
        movePattern.ActivateMoveToTarget(searchPattern.GetLastTargetPoint());
        currentLoop = SearchStateLoop;
    }
    private void SearchStateLoop()
    {
        searchPattern.SearchLoop();
        movePattern.CorrectTarget(searchPattern.GetLastTargetPoint());

        if(TargetsInView())
        {
            ToHuntingState();
            return;
        }
        else if (searchPattern.NeedStopSearch)
        {
            ToPatrolState();
        }
    }

    private bool TargetsInView()
    {
        curentVisibleTargets.Clear();

        if (viewPattern.TargetIsVisible(player))
        {
            curentVisibleTargets.Add(player);
        }

        foreach (var otherTarget in choseTargetPattern.otherTargets)
        {
            if(otherTarget == null) continue;
            if (viewPattern.TargetIsVisible(otherTarget))
            {
                curentVisibleTargets.Add(otherTarget);
            }
        }

        if (curentVisibleTargets.Count > 0)
        {
            ToHuntingState();
            return true;
        }
        return false;
    }

    public override void GetDamage(int damage)
    {
        OnGetDamage();
        base.GetDamage(damage);
    }

    private void OnGetDamage()
    {
        searchPattern.UpdateLastTargetPoint(player.position);
        ToSearchState(player);
    }
    protected override void Dead()
    {
        DisableAllMarkers();
        AudioPack.audioSystem.SoundEventInPoint -= OnSoundEvent;
        AudioPack.audioSystem.PlaySoundInPoint(deadClip, transform.position, 50);
        movePattern.StopMove();
        for (int i = attackPattern.turrets.Count-1; i >= 0; i--)
        {
            BotTurret t = attackPattern.turrets[i];
            if (t != null)
            {
                t.GetDamage(1000);
            }
        }
        Instantiate(deadExplosionPrefab, transform.position, Quaternion.identity);
        base.Dead();
    }
    private void OnSoundEvent(Vector3 point, float range)
    {
        float enemyDistance = Vector3.Distance(point, transform.position);
        if (range  + hearingLevel > enemyDistance)
        {
            searchPattern.UpdateLastTargetPoint(point);
            movePattern.agent.isStopped = false;
            ToSearchState(point);
        }
    }
}

[Serializable]
public class MovePattern
{
    [Min(0.01f)]
    public float moveSpeed = 1;
    public NavMeshAgent agent;


    public void StopMove()
    {
        agent.isStopped = true;
    }
    public void ActivateMoveToTarget(Vector3 target)
    {
        agent.isStopped = false;
        agent.destination = target;
    }
    public void CorrectTarget(Vector3 newTarget)
    {
        agent.destination = newTarget;
    }

    public bool NearWithPoint(Vector3 point)
    {
        return Vector3.SqrMagnitude(point - agent.transform.position) <= (agent.radius * agent.radius)*4;
    }


}

[Serializable]
public class ViewPattern
{
    public List<Transform> CurrentVisibleTargets { get; private set; } = new List<Transform>();

    [SerializeField]
    private Transform viewPoint;
    [SerializeField, Min(1)]
    private float viewDistance = 10;
    [SerializeField, Range(1, 360)]
    private float viewAngle = 1;
    [SerializeField]
    private LayerMask ignoreMask;

    public bool TargetIsVisible(Transform targetTransform)
    {
        Vector3 target = targetTransform.position;

        Vector3 dirToTarget = target - viewPoint.position;

        if(DistanceCheck(dirToTarget) && AngleCheck(dirToTarget) && ObstacleCkeck(targetTransform))
        {
            if(!CurrentVisibleTargets.Contains(targetTransform))
            {
                CurrentVisibleTargets.Add(targetTransform);
            }
            return true;
        }

        CurrentVisibleTargets.Remove(targetTransform);
        return false;
    }

    private bool ObstacleCkeck(Transform target)
    {
        if (Physics.Linecast(viewPoint.position, target.position, out RaycastHit hitInfo, ~ignoreMask))
        {
            if (hitInfo.collider.CompareTag(TagHolder.Player) || 
                hitInfo.collider.CompareTag(TagHolder.Allies))
            {
                return true;
            }
        }
        return false;
    }
    private bool AngleCheck(Vector3 direction)
    {
        direction.y = 0;
        return Vector3.Angle(viewPoint.forward, direction) <= viewAngle/2;
    }
    private bool DistanceCheck(Vector3 direction)
    {
        return direction.sqrMagnitude <= viewDistance * viewDistance;
    }
}

[Serializable]
public class ChoseTargetPattern
{
    public List<Transform> otherTargets;
    [SerializeField]
    private Transform transform;

    private float attackDistance;

    public void SetUp(float attackDistance)
    {
        this.attackDistance = attackDistance;
    }

    private List<EnemyTargetItem> targets = new List<EnemyTargetItem>();
    public Transform GetCurrentTarget(List<Transform> allVisibleTargets)
    {
        targets.Clear();
        foreach (Transform target in allVisibleTargets)
        {
            EnemyTargetItem targetItem = new EnemyTargetItem();
            targetItem.Target = target;

            float distance = Vector3.SqrMagnitude(target.position - transform.position);

            targetItem.order = distance;
            if(distance <= attackDistance)
            {
                targetItem.order /= 4;
            }
            if(targetItem.Target.CompareTag(TagHolder.Player))
            {
                targetItem.order /= 4;
            }

            targets.Add(targetItem);
        }

        targets.Sort();

        return targets[0].Target;
    }
}
public class EnemyTargetItem : IComparable<EnemyTargetItem>
{
    public Transform Target;
    public float order;

    public int CompareTo(EnemyTargetItem other)
    {
        if (order > other.order)
        {
            return -1;
        }
        else if (order < other.order)
        {
            return 1;
        }
        return 0;
    }
}

[Serializable]
public class SearchPattern
{
    [SerializeField, Min(1)]
    private float searchTime = 1;
    [SerializeField, Min(1)]
    private float searchPredictionTime = 1;

    private float currentSearchTime = 0;
    private Transform lastTarget;
    private Vector3 lastTargetPoint;

    public bool NeedStopSearch { get; private set; }

    public void UpdateLastTarget(Transform target)
    {
        NeedStopSearch = false;
        lastTarget = target;
        lastTargetPoint = target.position;
        ResetCurrentSearchTime();
    }
    public void UpdateLastTargetPoint(Vector3 targetPoint)
    {
        NeedStopSearch = false;
        lastTargetPoint = targetPoint;
        ResetCurrentSearchTime();
    }
    public void SearchLoop()
    {
        if(lastTarget != null)
        {
            lastTargetPoint = lastTarget.position;
        }

        currentSearchTime += Time.deltaTime;

        if(currentSearchTime >= searchPredictionTime)
        {
            lastTarget = null;
        }
        if (currentSearchTime >= searchTime)
        {
            NeedStopSearch = true;
        }
    }
    public Vector3 GetLastTargetPoint()
    { 
        return lastTargetPoint;
    }
    private void ResetCurrentSearchTime()
    {
        currentSearchTime = 0;
    }
}

[Serializable]
public class PatrollPattern
{
    public bool TimeToStateInPoint { get; private set; } = false;
    [SerializeField]
    private Transform[] patrolPoints;
    [SerializeField, Min(1)]
    private float patrolStayTime = 1;
    private int currentTargetPatrolPont = 0;
    private int patrolPointChangeMultiplier = 1;
    private float currentPatrolStayTime;

    public bool ContainsPatrolPoints()
    {
        return patrolPoints.Length > 0;
    }
    public Transform GetTargetPatrolPoint()
    {
        return patrolPoints[currentTargetPatrolPont];
    }
    public void StayInPointLoop()
    {
        if(!TimeToStateInPoint)
        {
            return;
        }

        currentPatrolStayTime += Time.deltaTime;
        if (currentPatrolStayTime >= patrolStayTime)
        {
            TimeToStateInPoint = false;
            SetNextPatrolPoint();
        }
    }
    public void AgentInPatrolPoint()
    {
        TimeToStateInPoint = true;
    }

    private void SetNextPatrolPoint()
    {
        currentTargetPatrolPont += patrolPointChangeMultiplier;

        if (currentTargetPatrolPont < 0)
        {
            currentTargetPatrolPont = 1;
            patrolPointChangeMultiplier = 1;
        }
        else if (currentTargetPatrolPont > patrolPoints.Length - 1)
        {
            currentTargetPatrolPont = patrolPoints.Length - 2;
            patrolPointChangeMultiplier = -1;
        }
        currentPatrolStayTime = 0;
    }
}

[Serializable]
public class AttackPattern
{
    [Min(1)]
    public float attackDistance = 10;
    public List<BotTurret> turrets;

    public bool TargetInAttackDistance(Transform actor, Vector3 target)
    {
        Vector3 dir = target - actor.position;
        return dir.sqrMagnitude < attackDistance*attackDistance;
    }
    public void UseWeapon(Vector3 target)
    {
        foreach (var t in turrets)
        {
            if (t == null) continue;
            t.Aim(target);
            t.TryShoot();
        }
    }
    public void OnPartDestroy(EnemyPart turret)
    {
        turrets.Remove(turret as BotTurret);
    }
}

[Serializable]
public class InteractionPattern
{
    [SerializeField, Min(1)]
    private float interactiveLoopTime = 1;
    [SerializeField, Min(1)]
    private float interactionDistance = 1;
    [SerializeField]
    private LayerMask interactionMask;

    private float interactionTimer = 0;

    public void CheckManualInteractive(Transform actor)
    {
        interactionTimer += Time.deltaTime;

        if (interactionTimer < interactiveLoopTime)
            return;

        Collider[] colliders = Physics.OverlapSphere(actor.position, interactionDistance, interactionMask);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out ManualInteractive component))
            {
                component.NPC_Use();
            }
        }

        interactionTimer = 0;
    }
}