using UnityEngine;

[CreateAssetMenu(fileName = "ITM_New", menuName = "Course/Data/Item")]
public class ItemDefinition : ScriptableObject
{
    [SerializeField] private string id = "new_item";
    [SerializeField] private string displayName = "New Item";
    [SerializeField] private Sprite icon;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField, Min(1)] private int scoreValue = 1;

    public string Id => id;
    public string DisplayName => displayName;
    public Sprite Icon => icon;
    public int ScoreValue => scoreValue;
    public AudioClip PickupSound => pickupSound;

    private void OnValidate()
    {
        id = string.IsNullOrWhiteSpace(id) ? "new_item" : id.Trim().ToLowerInvariant().Replace(" ", "_");
        scoreValue = Mathf.Max(1, scoreValue);
    }
}
