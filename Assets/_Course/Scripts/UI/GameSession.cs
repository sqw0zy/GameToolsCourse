using System;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [SerializeField, Min(1)] private int maxHealth = 100;
    [SerializeField, Min(1f)] private float levelDuration = 90f;
    [SerializeField, Min(1)] private int requiredScore = 5;

    public static GameSession instance;

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
        instance = this;
        Health = maxHealth;
        RemainingTime = levelDuration;
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
        if (Score >= requiredScore) Finish(true);
    }

    public void ApplyDamage(int amount)
    {
        if (IsFinished || amount <= 0) return;
        Health = Mathf.Max(0, Health - amount);
        HealthChanged?.Invoke(Health, maxHealth);
        if (Health <= 0) Finish(false);
    }

    public void PublishState()
    {
        ScoreChanged?.Invoke(Score);
        HealthChanged?.Invoke(Health, maxHealth);
        TimeChanged?.Invoke(RemainingTime);
    }
    
    private void Finish(bool victory)
    {
        if (IsFinished) return;
        IsFinished = true;
        Finished?.Invoke(victory, Score);
    }
}
