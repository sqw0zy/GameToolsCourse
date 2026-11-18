using System;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [SerializeField] private DifficultyProfile profile;

    public static GameSession instance;
    public DifficultyProfile Profile => profile;

    public event Action<int> ScoreChanged;
    public event Action<int, int> HealthChanged;
    public event Action<float> TimeChanged;
    public event Action<bool, int> Finished;

    public bool IsFinished { get; private set; }
    public int Score { get; private set; }
    public int Health { get; private set; }
    public float RemainingTime { get; private set;  }

    private void Awake()
    {
        if (profile is null)
        {
            Debug.LogError("DifficultyProfile is not assigned.", this);
            enabled = false;
            return;
        }
        Score = 0;
        instance = this;
        Health = profile.PlayerMaxHealth;
        RemainingTime = profile.LevelDuration;
    }

    private void Start()
    {
        PublishState();
    }

    private void Update()
    {
        if (IsFinished) return;
        RemainingTime = Mathf.Max(0f, RemainingTime - Time.deltaTime);
        TimeChanged?.Invoke(RemainingTime);
        if (RemainingTime <= 0f) Finish(false);
    }
    public void AddScore(int amount)
    {
        if (IsFinished || amount <= 0) return;
        Score += amount;
        ScoreChanged?.Invoke(Score);
        if (Score >= profile.RequiredScore) Finish(true);
    }

    public void ApplyDamage(int amount)
    {
        if (IsFinished || amount <= 0) return;
        Health = Mathf.Max(0, Health - amount);
        HealthChanged?.Invoke(Health, profile.PlayerMaxHealth);
        if (Health <= 0) Finish(false);
    }

    public void PublishState()
    {
        ScoreChanged?.Invoke(Score);
        HealthChanged?.Invoke(Health, profile.PlayerMaxHealth);
        TimeChanged?.Invoke(RemainingTime);
    }
    
    private void Finish(bool victory)
    {
        if (IsFinished) return;
        IsFinished = true;
        Finished?.Invoke(victory, Score);
    }
}
