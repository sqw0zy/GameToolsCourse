using UnityEngine;

[CreateAssetMenu(fileName = "DIFF_New", menuName = "Course/Data/Difficulty Profile")]
public class DifficultyProfile : ScriptableObject
{
    [SerializeField] private string displayName = "Normal";
    [SerializeField, Min(1)] private int playerMaxHealth = 100;
    [SerializeField, Min(1f)] private float levelDuration = 90f;
    [SerializeField, Min(1)] private int requiredScore = 5;
    [SerializeField] private NpcConfig npcConfig;

    public string DisplayName => displayName;
    public int PlayerMaxHealth => playerMaxHealth;
    public float LevelDuration => levelDuration;
    public int RequiredScore => requiredScore;
    public NpcConfig NPCConfig => npcConfig;

    private void OnValidate()
    {
        playerMaxHealth = Mathf.Max(1, playerMaxHealth);
        levelDuration = Mathf.Max(1f, levelDuration);
        requiredScore = Mathf.Max(1, requiredScore);
    }
}
