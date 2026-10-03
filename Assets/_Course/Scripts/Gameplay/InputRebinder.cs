using UnityEngine;
using UnityEngine.InputSystem;

public class InputRebinder : MonoBehaviour
{
    private const string RebindsKey = "input-rebinds";

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private string actionName = "Interact";
    [SerializeField, Min(0)] private int bindingIndex;

    private InputActionRebindingExtensions.RebindingOperation operation;

    private void Awake()
    {
        if (playerInput == null) playerInput = GetComponent<PlayerInput>();

        string json = PlayerPrefs.GetString(RebindsKey, string.Empty);
        if (!string.IsNullOrEmpty(json))
        {
            playerInput.actions.LoadBindingOverridesFromJson(json);
        }
    }

    public void OnBeginRebind(InputAction.CallbackContext context)
    {
        if (!context.performed || operation != null) return;

        InputAction action = playerInput.actions.FindAction(actionName, true);
        action.Disable();

        operation = action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("<Mouse>/position")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(_ => FinishRebind(action, true))
            .OnCancel(_ => FinishRebind(action, false))
            .Start();
    }

    public void ResetBinding()
    {
        InputAction action = playerInput.actions.FindAction(actionName, true);
        action.RemoveBindingOverride(bindingIndex);
        SaveOverrides();
    }

    private void FinishRebind(InputAction action, bool save)
    {
        operation.Dispose();
        operation = null;
        action.Enable();

        if (save) SaveOverrides();
    }

    private void SaveOverrides()
    {
        string json = playerInput.actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(RebindsKey, json);
        PlayerPrefs.Save();
    }

    private void OnDispose()
    {
        operation?.Dispose();
    }
}
