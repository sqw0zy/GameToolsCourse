using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    [Header("Model")]
    [SerializeField] private GameSession session;
    [SerializeField] private PlayerInput playerInput;

    [Header("HUD")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Slider healthSlider;

    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private TMP_Text resultTitle;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject restartButton;

    private bool paused;
    private Coroutine mapSwitchCoroutine;

    private void OnEnable()
    {
        session.ScoreChanged += UpdateScore;
        session.HealthChanged += UpdateHealth;
        session.TimeChanged += UpdateTime;
        session.Finished += ShowResult;
    }

    private void Start()
    {
        pausePanel.SetActive(false);
        endPanel.SetActive(false);
        Time.timeScale = 1.0f;

        if (playerInput != null) playerInput.SwitchCurrentActionMap("Gameplay");

        session.PublishState();
    }

    private void OnDisable()
    {
        session.ScoreChanged -= UpdateScore;
        session.HealthChanged -= UpdateHealth;
        session.TimeChanged -= UpdateTime;
        session.Finished -= ShowResult;
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed && !session.IsFinished) SetPaused(true);
    }
    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.performed && paused) SetPaused(false);
    }
    public void ResumeGame()
    {
        if (paused) SetPaused(false);
    }
    
    private void SetPaused(bool value)
    {
        paused = value;
        pausePanel.SetActive(value);
        Time.timeScale = value ? 0f : 1f;

        if (mapSwitchCoroutine != null ) StopCoroutine(mapSwitchCoroutine);
        mapSwitchCoroutine = StartCoroutine(SwitchActionMapSafe(value ? "UI" : "Gameplay"));
        //playerInput.SwitchCurrentActionMap(value ? "UI" : "Gameplay");
        Cursor.visible = value;
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        if (value) UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(resumeButton);
    }

    private IEnumerator SwitchActionMapSafe(string mapName)
    {
        yield return null;

        if (playerInput != null)
        {
            playerInput.SwitchCurrentActionMap(mapName);
        }
    }

    private void UpdateScore(int value)
    {
        scoreText.text = $"Счет: {value}";
    }
    private void UpdateHealth(int current, int maximum)
    {
        healthSlider.maxValue = maximum;
        healthSlider.value = current;
        healthText.text = $"{current} / {maximum}";
    }
    private void UpdateTime(float seconds)
    {
        int total = Mathf.CeilToInt(seconds);
        timerText.text = $"{total / 60:00}:{total % 60:00}";
    }
    
    private void ShowResult(bool victory, int score)
    {
        paused = false;
        pausePanel.SetActive(false);
        endPanel.SetActive(true);
        resultTitle.text = victory ? "Победа" : "Поражение";
        finalScoreText.text = $"Итоговый счет: {score}";
        Time.timeScale = 0f;
        playerInput.SwitchCurrentActionMap("UI");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(restartButton);
    }
}
