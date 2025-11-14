using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FightBot : MultipartEnemy
{
    [SerializeField, Min(1)]
    private float speed = 1;
    [SerializeField, Min(1)]
    private float viewDistance = 10;
    [SerializeField, Min(1)]
    private float stopMoveDistance = 10;
    [SerializeField, Min(1)]
    private float stopFindingDistance = 10;
    [SerializeField, Range(1, 360)]
    private float viewAngle = 1;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private LayerMask ignoreMask;
    [SerializeField]
    private List<BotTurret> turrets;
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private Transform viewPoint;
    [SerializeField, Min(0)]
    private float findingPredictionTime = 1;
    [SerializeField]
    private GameObject deadExplosionPrefab;

    [SerializeField]
    private GameObject[] patternMarkers;
    [SerializeField]
    private Transform[] patrolPoints;
    [SerializeField, Min(1)]
    private float patrolStayTime = 1;

    [SerializeField]
    private AudioClip deadClip;

    private Vector3 lastPlayerPoint;

    private int currentTargetPatrolPont;
    private int patrolPointChangeMultiplier = 1;
    private float currentPatrolStayTime;

    private Action currentPattern;
    private float currentFindingTime;
    private bool viewPlayer = false;

    private void Start()
    {
        currentTargetPatrolPont = -1;
        agent.isStopped = true;
        SetMarker(0);
        currentPattern = StayPattern;

        foreach(BotTurret turret  in turrets)
        {
            turret.OnDestroyPart += OnPartDestroy;
            turret.DamageEvent += OnGetDamage;
        }

        AudioPack.audioSystem.SoundEventInPoint += OnSoundEvent;

    }

    private void Update()
    {
        if(HP > 0)
            currentPattern();
    }

    private void OnPartDestroy(EnemyPart turret)
    {
        turrets.Remove(turret as BotTurret);
    }

    private void SetMarker(int index)
    {
        foreach (GameObject t in patternMarkers)
        {
            t.SetActive(false);
        }
        patternMarkers[index].SetActive(true);
    }

    private void StayPattern()
    {
        if (PlayerVisible())
        {
            Vector3 dirToPlayer = target.position - transform.position;

            if (dirToPlayer.sqrMagnitude <= viewDistance * viewDistance)
            {
                viewPlayer = true;
                agent.isStopped = false;
                SetMarker(1);
                currentPattern = MoveToPlayerPattern;
            }
        }

        currentPatrolStayTime += Time.deltaTime;
        if (currentPatrolStayTime >= patrolStayTime)
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
            agent.isStopped = false;
            currentPatrolStayTime = 0;
            currentPattern = MoveToPatrolPointPattern;
        }
    }

    private void MoveToPatrolPointPattern()
    {
        if (PlayerVisible())
        {
            Vector3 dirToPlayer = target.position - transform.position;

            if (dirToPlayer.sqrMagnitude <= viewDistance * viewDistance)
            {
                viewPlayer = true;
                agent.isStopped = false;
                SetMarker(1);
                currentPattern = MoveToPlayerPattern;
            }
        }
        else
        {
            Transform patrolPoint = patrolPoints[currentTargetPatrolPont];
            Vector3 dirToPoint = patrolPoint.position - transform.position;
            agent.destination = patrolPoint.position;

            if (dirToPoint.sqrMagnitude < stopMoveDistance * stopMoveDistance)
            {
                agent.isStopped = true;
                SetMarker(0);
                currentPattern = StayPattern;
                return;
            }
        }
    }

    private void MoveToPlayerPattern()
    {
        if (PlayerVisible())
        {
            Vector3 dirToPlayer = target.position - transform.position;

            if (dirToPlayer.sqrMagnitude > viewDistance * viewDistance)
            {
                viewPlayer = true;
                agent.isStopped = false;
                SetMarker(3);
                currentFindingTime = 0;
                currentPattern = FindingTargetPattern;
            }
            else if (dirToPlayer.sqrMagnitude < stopMoveDistance * stopMoveDistance)
            {
                agent.isStopped = true;
                SetMarker(2);
                currentPattern = AttackPattern;
            }
            else
            {
                agent.destination = target.position;
            }
        }
        else
        {
            viewPlayer = true;
            agent.isStopped = false;
            SetMarker(3);
            currentFindingTime = 0;
            currentPattern = FindingTargetPattern;
        }
    }

    private void AttackPattern()
    {
        UseWeapon();

        if (PlayerVisible())
        {
            Vector3 dirToPlayer = target.position - transform.position;

            agent.transform.forward = Vector3.Lerp(agent.transform.forward, dirToPlayer, Time.deltaTime);

            if (dirToPlayer.sqrMagnitude > stopMoveDistance * stopMoveDistance)
            {
                agent.isStopped = false;
                SetMarker(1);
                currentPattern = MoveToPlayerPattern;
            }
        }
        else
        {
            viewPlayer = true;
            agent.isStopped = false;
            SetMarker(3);
            currentFindingTime = 0;
            currentPattern = FindingTargetPattern;
        }
    }

    private void FindingTargetPattern()
    {
        if (viewPlayer)
        {
            if (currentFindingTime < findingPredictionTime)
            {
                currentFindingTime += Time.deltaTime;
                lastPlayerPoint = target.position;
            }
            else
            {
                viewPlayer = false;
            }
        }

        agent.destination = lastPlayerPoint;

        if (PlayerVisible())
        {
            Vector3 dirToPlayer = target.position - transform.position;

            if (dirToPlayer.sqrMagnitude <= viewDistance * viewDistance)
            {
                agent.isStopped = false;
                SetMarker(1);
                currentPattern = MoveToPlayerPattern;
            }
        }
        else
        {
            Vector3 dirToPlayer = lastPlayerPoint - transform.position;

            if (dirToPlayer.sqrMagnitude <= stopFindingDistance * stopFindingDistance)
            {
                agent.isStopped = true;
                SetMarker(0);
                currentPattern = StayPattern;
                Debug.Log("Закончить поиск");
            }
        }
    }

    private void RotateToShoot()
    {
        Vector3 dir = lastPlayerPoint - agent.transform.position;

        agent.transform.forward = Vector3.MoveTowards(agent.transform.forward, dir, Time.deltaTime * 2);

        if(Vector3.Angle(dir, agent.transform.forward) <= viewAngle/3)
        {
            agent.isStopped = false;
            currentPattern = FindingTargetPattern;
        }
    }

    private bool PlayerVisible()
    {
        Vector3 dirToPlayer = target.position - viewPoint.position;

        if(Vector3.Angle(transform.forward, dirToPlayer) > viewAngle/2)
            return false;

        Debug.DrawLine(viewPoint.position, target.position, Color.red);


        if (Physics.Linecast(viewPoint.position, target.position, out RaycastHit hitInfo, ~ignoreMask))
        {
            if (hitInfo.collider.CompareTag("Player"))
            {
                lastPlayerPoint = target.position;
                return true;
            }
        }
        return false;
    }

    private void UseWeapon()
    {
        foreach (var t in turrets)
        {
            t.Aim(lastPlayerPoint);
            t.TryShoot();
        }
    }

    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);
        OnGetDamage();
    }

    private void OnGetDamage()
    {
        lastPlayerPoint = target.position;
        currentPattern = RotateToShoot;
    }

    protected override void Dead()
    {
        AudioPack.audioSystem.SoundEventInPoint -= OnSoundEvent;
        AudioPack.audioSystem.PlaySoundInPoint(deadClip, transform.position, 50);
        agent.isStopped = true;
        for (int i = turrets.Count-1; i >= 0; i--)
        {
            BotTurret t = turrets[i];
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
        if(Vector3.Distance(point, transform.position) < range)
        {
            lastPlayerPoint = point;
            agent.isStopped = false;
            SetMarker(3);
            currentFindingTime = 0;
            currentPattern = FindingTargetPattern;
        }
    }
}
