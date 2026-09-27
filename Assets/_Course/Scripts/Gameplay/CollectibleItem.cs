using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

[RequireComponent (typeof(Collider))]
public class CollectibleItem : MonoBehaviour
{
    [SerializeField, Min(1)] private int scoreValue = 1;

    private bool collected;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;
        if (!other.TryGetComponent(out PlayerState playerState)) return;

        collected = true;
        playerState.AddScore(scoreValue);
        gameObject.SetActive(false);
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
