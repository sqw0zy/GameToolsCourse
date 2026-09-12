using UnityEngine;

public class OscillatingObject : MonoBehaviour
{
    [SerializeField] private Vector3 axis = Vector3.up;
    [SerializeField, Min(0f)] private float amplitude = .5f;
    [SerializeField, Min(0f)] private float frequency = 1f;

    private Vector3 startLocalPosition;

    private void Awake()
    {
        startLocalPosition = transform.localPosition;
    }

    private void Start()
    {
        Debug.Log($"{name}:local = {transform.localPosition}; world = {transform.position}");
    }

    private void Update()
    {
        float phase = Time.time * frequency * 2f * Mathf.PI;
        Vector3 direction = axis.sqrMagnitude > 0f ? axis.normalized : Vector3.zero;
        transform.localPosition = startLocalPosition + direction * Mathf.Sin(phase) * amplitude;
    }
}
