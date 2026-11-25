using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KinematicPlatform : MonoBehaviour
{
    [SerializeField] private Vector3 localOffset = new Vector3(0f, 0f, 4f);
    [SerializeField, Min(0.1f)] private float cycleDuration = 4f;

    private Rigidbody body;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float elapsed;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.isKinematic = true;
        body.interpolation = RigidbodyInterpolation.Interpolate;
        startPosition = body.position;
        targetPosition = startPosition + transform.TransformVector(localOffset);
    }

    private void FixedUpdate()
    {
        elapsed += Time.fixedDeltaTime;
        float phase = Mathf.PingPong(elapsed * 2f / cycleDuration, 1f);
        body.MovePosition(Vector3.Lerp(startPosition, targetPosition, phase));
    }
}
