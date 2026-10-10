using UnityEngine;

public class CollisionProbe : MonoBehaviour
{
    [SerializeField, Min(0f)] private float reportThreshold = .5f;

    private void OnCollisionEnter(Collision collision)
    {
        float speed = collision.relativeVelocity.magnitude;
        if (speed < reportThreshold) return;

        Vector3 normal = collision.contactCount > 0 ? collision.GetContact(0).normal : Vector3.zero;

        Debug.Log(
            $"Collision with {collision.gameObject.name};" +
            $"relative speed = {speed:F2}; normal = {normal}", this);
    }
}