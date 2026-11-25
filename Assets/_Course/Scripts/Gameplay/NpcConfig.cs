using UnityEngine;

[CreateAssetMenu(fileName = "NPC_New", menuName = "Course/Data/NPC Config")]
public class NpcConfig : ScriptableObject
{
    [SerializeField, Min(.1f)] private float patrolSpeed = 2f;
    [SerializeField, Min(.1f)] private float chaseSpeed = 4f;
    [SerializeField, Min(0f)] private float arrivalTolerance = .25f;
    [SerializeField, Min(.1f)] private float detectionDistance = 6f;
    [SerializeField, Min(.1f)] private float loseDistance = 10f;
    [SerializeField, Min(0f)] private float animationDamp = .12f;

    public float PatrolSpeed => patrolSpeed;
    public float ChaseSpeed => chaseSpeed;
    public float ArrivalTolerance => arrivalTolerance;
    public float DetectionDistance => detectionDistance;
    public float LoseDistance => loseDistance;
    public float AnimationDamp => animationDamp;

    private void OnValidate()
    {
        patrolSpeed = Mathf.Max(.1f, patrolSpeed);
        chaseSpeed = Mathf.Max(patrolSpeed, chaseSpeed);
        arrivalTolerance = Mathf.Max(0f, arrivalTolerance);
        detectionDistance = Mathf.Max(.1f, detectionDistance);
        loseDistance = Mathf.Max(detectionDistance + .5f, loseDistance);
        animationDamp = Mathf.Max(0f, animationDamp);
    }
}
