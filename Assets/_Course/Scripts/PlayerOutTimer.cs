using UnityEngine;

public class PlayerOutTimer : MonoBehaviour
{
    private float timer;
    private bool stopTimer = false;
    private void Awake()
    {
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (stopTimer)
        {
            Debug.Log($"time: {timer} seconds");
        }
        else
            timer += Time.deltaTime;
    }

    

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out PlayerState playerState)) return;
        stopTimer = true;
    }
}
