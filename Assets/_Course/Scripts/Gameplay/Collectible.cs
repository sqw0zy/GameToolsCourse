using Unity.VectorGraphics;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Collectible : MonoBehaviour
{
    [SerializeField, Min(1)] private int value = 1;
    private AudioSource AudioSource;
    private bool collected;

    private void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected || !other.CompareTag("Player")) return;

        collected = true;
        GameSession.instance.AddScore(1);

        AudioSource.Play();

        GetComponent<Collider>().enabled= false;
        MeshRenderer mesh = GetComponentInChildren<MeshRenderer>();
        mesh.enabled = false;

        Debug.Log($"Collected {name}; value = {value}", this);
        //gameObject.SetActive(false);
        Destroy(gameObject, 2f);
    }
}
