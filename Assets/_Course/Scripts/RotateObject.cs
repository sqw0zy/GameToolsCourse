using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float degreesPerSecond = 60f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, degreesPerSecond * Time.deltaTime, Space.World);
    }
}
