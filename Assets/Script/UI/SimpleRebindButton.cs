using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

/// <summary>
/// 简单的重绑定按钮 - 直接操作 InputManager 的 playerActions，改键后立即生效
/// </summary>
public class SimpleRebindButton : MonoBehaviour
{
    [Header("配置")]
    [Tooltip("要重绑定的动作名称，例如: Jump, Attack, Dash 等")]
    public string actionName = "Jump";
    
    [Tooltip("绑定索引，通常为 0")]
    public int bindingIndex = 0;
    
    [Header("UI 引用")]
    [Tooltip("显示当前按键的文本")]
    public TextMeshProUGUI bindingText;
    
    [Tooltip("重绑定按钮")]
    public Button rebindButton;
    
    private InputAction action;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private void Start()
    {
        if (rebindButton != null)
        {
            rebindButton.onClick.AddListener(StartRebinding);
        }
        
        UpdateBindingDisplay();
    }

    private void OnEnable()
    {
        UpdateBindingDisplay();
    }

    /// <summary>
    /// 更新显示当前按键
    /// </summary>
    public void UpdateBindingDisplay()
    {
        if (InputManager.instance == null || bindingText == null)
            return;

        // 根据动作名称获取对应的 InputAction
        action = GetActionByName(actionName);
        
        if (action != null && bindingIndex < action.bindings.Count)
        {
            bindingText.text = InputControlPath.ToHumanReadableString(
                action.bindings[bindingIndex].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice
            );
        }
    }

    /// <summary>
    /// 开始重绑定
    /// </summary>
    public void StartRebinding()
    {
        if (InputManager.instance == null)
        {
            Debug.LogError("InputManager not found!");
            return;
        }

        action = GetActionByName(actionName);
        
        if (action == null)
        {
            Debug.LogError($"Action '{actionName}' not found!");
            return;
        }

        if (bindingText != null)
        {
            bindingText.text = "按任意键...";
        }

        // 禁用按钮
        if (rebindButton != null)
        {
            rebindButton.interactable = false;
        }

        // 禁用 action（重绑定期间必须禁用）
        action.Disable();

        // 开始重绑定
        rebindingOperation = action.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(operation => OnRebindComplete())
            .OnCancel(operation => OnRebindCancelled())
            .Start();
    }

    private void OnRebindComplete()
    {
        rebindingOperation?.Dispose();
        rebindingOperation = null;

        // 重新启用 action
        action?.Enable();

        // 启用按钮
        if (rebindButton != null)
        {
            rebindButton.interactable = true;
        }

        // 更新显示
        UpdateBindingDisplay();

        // 保存绑定
        if (KeyRebindingManager.instance != null)
        {
            KeyRebindingManager.instance.SaveBindings();
        }
    }

    private void OnRebindCancelled()
    {
        rebindingOperation?.Dispose();
        rebindingOperation = null;

        // 重新启用 action
        action?.Enable();

        // 启用按钮
        if (rebindButton != null)
        {
            rebindButton.interactable = true;
        }

        // 恢复显示
        UpdateBindingDisplay();
    }

    /// <summary>
    /// 重置为默认绑定
    /// </summary>
    public void ResetToDefault()
    {
        if (action != null && bindingIndex < action.bindings.Count)
        {
            action.RemoveBindingOverride(bindingIndex);
            UpdateBindingDisplay();

            // 保存
            if (KeyRebindingManager.instance != null)
            {
                KeyRebindingManager.instance.SaveBindings();
            }
        }
    }

    private InputAction GetActionByName(string name)
    {
        if (InputManager.instance == null || InputManager.instance.playerActions == null)
            return null;

        var playerActions = InputManager.instance.playerActions.Player;

        return name switch
        {
            "Movement" => playerActions.Movement,
            "Jump" => playerActions.Jump,
            "Attack" => playerActions.Attack,
            "CounterAttack" => playerActions.CounterAttack,
            "Dash" => playerActions.Dash,
            "Pause" => playerActions.Pause,
            _ => null
        };
    }

    private void OnDestroy()
    {
        rebindingOperation?.Dispose();
    }
}
