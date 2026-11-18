using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class NpcNavigator : MonoBehaviour
{
    private enum State { Patrol, Chase, Return}

    [Header("Links")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] patrolPoints;

    [Header("Configuration")]
    [SerializeField] private NpcConfig config;

    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int ChaseHash = Animator.StringToHash("IsChasing");

    private NavMeshAgent agent;
    private Animator animator;
    private State state;
    private int patrolIndex;
    private Vector3 homePosition;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        homePosition = transform.position;
    }

    private void Start()
    {
        if (config == null)
        {
            Debug.LogError("NpcConfig is not assigned.", this);
            enabled = false;
            return;
        }

        EnterPatrol();
    }

    private void Update()
    {
        if (!agent.isOnNavMesh || player == null)
        {
            animator.SetFloat(SpeedHash, 0f, config.AnimationDamp, Time.deltaTime);
            animator.SetBool(ChaseHash, false);
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch(state)
        {
            case State.Patrol:
                if (distanceToPlayer <= config.DetectionDistance)
                    EnterChase();
                else if (HasArrived())
                    MoveToNextPoint();
                break;
            case State.Chase:
                agent.SetDestination(player.position);
                if (distanceToPlayer >= config.LoseDistance)
                    EnterReturn();
                break;
            case State.Return:
                if (distanceToPlayer <= config.DetectionDistance)
                    EnterChase();
                else if (HasArrived())
                    EnterPatrol();
                break;
        }

        float normalizedSpeed = Mathf.Clamp01(agent.velocity.magnitude / config.ChaseSpeed);
        animator.SetFloat(SpeedHash, normalizedSpeed, config.AnimationDamp, Time.deltaTime);
        animator.SetBool(ChaseHash, state == State.Chase);
    }

    private bool HasArrived()
    {
        if (agent.pathPending || agent.pathStatus != NavMeshPathStatus.PathComplete) 
            return false;

        return agent.remainingDistance <= agent.stoppingDistance + config.ArrivalTolerance;
    }

    private void EnterPatrol()
    {
        state = State.Patrol;
        agent.speed = config.PatrolSpeed;
        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[patrolIndex].position);
        }
    }

    private void MoveToNextPoint()
    {
        if (patrolPoints.Length == 0) return;

        patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
        agent.SetDestination(patrolPoints[patrolIndex].position);
    }

    private void EnterChase()
    {
        state = State.Chase;
        agent.speed = config.ChaseSpeed;
        agent.SetDestination(player.position);
    }

    private void EnterReturn()
    {
        state = State.Return;
        agent.speed = config.PatrolSpeed;
        agent.SetDestination(homePosition);
    }

    private void OnDrawGizmosSelected()
    {
        if (config == null)
        {
            Debug.LogError("config is not selected.", this);
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, config.DetectionDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, config.LoseDistance);
    }
}
