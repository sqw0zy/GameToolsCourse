using UnityEngine;

public class FastBody : MonoBehaviour
{
    public float testSpeed = 5f;
    private Rigidbody body;
    private int n = 0;
    private bool stopTimer = false;
    private float time;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        time = Time.time;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        body.linearVelocity = transform.forward * testSpeed;
    }
    void FixedUpdate()
    {
        if (stopTimer) { return; }
        if (time >= 1)
        {
            Debug.Log($"after one second fixedUpdate has been called {n} times");
            stopTimer = true;
        }
        n++;
        time += Time.fixedDeltaTime;
    }
}
