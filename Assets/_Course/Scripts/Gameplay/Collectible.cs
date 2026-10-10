using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Collectible : MonoBehaviour
{
    [SerializeField, Min(1)] private int value = 1;
    private bool collected;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected || !other.CompareTag("Player")) return;

        collected = true;
        Debug.Log($"Collected {name}; value = {value}", this);
        gameObject.SetActive(false);
    }
}
