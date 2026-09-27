using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageZone : MonoBehaviour
{
    [SerializeField, Min(1)] private int damage = 10;
    [SerializeField, Min(.1f)] private float cooldown = 1f;

    private float nextDamageTime;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (Time.time < nextDamageTime) return;
        if (!other.TryGetComponent(out PlayerState playerState)) return;

        playerState.TakeDamage(damage);
        //other.GetComponent<PlayerState>().TakeDamage(damage);
        nextDamageTime = Time.time + cooldown; 
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
