using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ConfigurablePickup : MonoBehaviour
{
    [SerializeField] private ItemDefinition definition;

    private GameSession session;
    private AudioSource audioSource;
    private bool collected;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (definition != null)
        {
            audioSource.generator = definition.PickupSound;
        }
    }
    private void Start()
    {
        session = GameSession.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected || definition == null || session == null) return;
        if (!other.CompareTag("Player")) return;
        collected = true;
        session.AddScore(definition.ScoreValue);

        if (definition.PickupSound != null)
        {
            audioSource.Play();
            //AudioSource.PlayClipAtPoint(definition.PickupSound, transform.position);
        }

        GetComponent<Collider>().enabled = false;
        MeshRenderer mesh = GetComponentInChildren<MeshRenderer>();
        mesh.enabled = false;

        Destroy(gameObject, 2f);
    }
}
