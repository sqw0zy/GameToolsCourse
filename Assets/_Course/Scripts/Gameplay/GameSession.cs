using Course.Gameplay;
using System;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [SerializeField] private DifficultyProfile profile;

    private ScoreCounter scoreCounter;

    //public static GameSession instance;
    public DifficultyProfile Profile => profile;

    public event Action<int> ScoreChanged;
    public event Action<int, int> HealthChanged;
    public event Action<float> TimeChanged;
    public event Action<bool, int> Finished;

    public bool IsFinished { get; private set; }
    public int Score => scoreCounter?.Score ?? 0;
    public int Health { get; private set; }
    public float RemainingTime { get; private set;  }

    private void Awake()
    {
        if (!ApplyProfile(profile))
        {
            Debug.LogError("DifficultyProfile is not assigned.", this);
            enabled = false;
            return;
        }
        //instance = this;
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

    public bool ApplyProfile(DifficultyProfile source)
    {
        if (source == null) return false;

        profile = source;
        scoreCounter = new ScoreCounter(source.RequiredScore);
        Health = source.PlayerMaxHealth;
        RemainingTime = source.LevelDuration;
        enabled = true;
        return true;
    }
    public void AddScore(int amount)
    {
        if (scoreCounter == null || amount <= 0) return;

        bool completed = scoreCounter.Add(amount);
        ScoreChanged?.Invoke(Score);
        if (completed) Finish(true);
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
