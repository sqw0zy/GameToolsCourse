using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DebugMover : MonoBehaviour
{
    [SerializeField] private Vector3 velocity = new(2, 0, 0);

    private Rigidbody body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 nextPosition = body.position + velocity * Time.fixedDeltaTime;
        body.MovePosition(nextPosition);


        //Debug.Log("Debug.Log in fixedUpdate");

        
    }
}
