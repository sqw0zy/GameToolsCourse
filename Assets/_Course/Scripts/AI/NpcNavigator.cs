using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class NpcNavigator : MonoBehaviour
{
    private enum State { Patrol, Chase, Return}

    [Header("Links")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] patrolPoints;

    [Header("Movement")]
    [SerializeField, Min(0.1f)] private float patrolSpeed = 2f;
    [SerializeField, Min(0.1f)] private float chaseSpeed = 4f;
    [SerializeField, Min(0f)] private float arrivalTolerance = .25f;

    [Header("Perception")]
    [SerializeField, Min(0.1f)] private float detectionDistance = 6f;
    [SerializeField, Min(.1f)] private float loseDistance = 10f;

    [Header("Animation")]
    [SerializeField, Min(0f)] private float animationDamp = .12f;

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

    }

    private void Update()
    {
        if (!agent.isOnNavMesh || player == null)
        {
            animator.SetFloat(SpeedHash, 0f, animationDamp, Time.deltaTime);
            animator.SetBool(ChaseHash, false);
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch(state)
        {
            case State.Patrol:
                if (distanceToPlayer <= detectionDistance)
                    EnterChase();
                else if (HasArrived())
                    MoveToNextPoint();
                break;
            case State.Chase:
                agent.SetDestination(player.position);
                if (distanceToPlayer >= loseDistance)
                    EnterReturn();
                break;
            case State.Return:
                if (distanceToPlayer <= detectionDistance)
                    EnterChase();
                else if (HasArrived())
                    EnterPatrol();
                break;
        }

        float normalizedSpeed = Mathf.Clamp01(agent.velocity.magnitude / chaseSpeed);
        animator.SetFloat(SpeedHash, normalizedSpeed, animationDamp, Time.deltaTime);
        animator.SetBool(ChaseHash, state == State.Chase);
    }

    private bool HasArrived()
    {
        if (agent.pathPending || agent.pathStatus != NavMeshPathStatus.PathComplete) 
            return false;

        return agent.remainingDistance <= agent.stoppingDistance + arrivalTolerance;
    }

    private void EnterPatrol()
    {
        state = State.Patrol;
        agent.speed = patrolSpeed;
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
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
    }

    private void EnterReturn()
    {
        state = State.Return;
        agent.speed = patrolSpeed;
        agent.SetDestination(homePosition);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, loseDistance);
    }
}
