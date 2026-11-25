using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField, Min(1)] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField, Min(0)] private int score;

    public int CurrentHealth => currentHealth;
    public int Score => score;
    public bool isAlive => currentHealth > 0;

    private void Awake()
    {
        currentHealth = maxHealth;
        Debug.Log($"{name}: Awake, health = {currentHealth}", this);
    }
    private void OnEnable()
    {
        Debug.Log($"{name}: OnEnable", this);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log($"{name}: Start", this);
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || !isAlive) return;

        currentHealth = Mathf.Max(0, currentHealth - amount);
        Debug.Log($"{name}: damage {amount}, health = {currentHealth}", this);
    }

    public void AddScore(int value)
    {
        if (value <= 0) return;

        score += value;
        Debug.Log($"{name}: score = {score}", this);
    }

    private void OnDisable()
    {
        Debug.Log($"{name}: OnDisable", this);
    }

    private void OnValidate()
    {
        maxHealth = Mathf.Max(1, maxHealth);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
}
