using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Samples.RebindUI; // 引用官方 UI

public class RebindInitializer : MonoBehaviour
{
    private void Start()
    {
        // 1. 获取 UI 组件
        var ui = GetComponent<RebindActionUI>();

        // 2. 获取 UI原本想改的动作名字 (比如 "Jump")
        // 注意：我们只用这个名字来定位
        string actionName = ui.actionReference.action.name;

        // 3. 从“唯一的井” (InputManager) 里找到真正的动作实例
        var realAction = InputManager.instance.playerActions.FindAction(actionName);


        if (realAction != null)
        {
            // 4. 【核心一步】
            // 官方文档允许我们为运行时动作创建一个新的 Reference
            // 我们把这个指向真身的 Reference 塞给 UI
            // 这样 UI 就会直接修改 InputManager，而不是改资源文件！
            ui.actionReference = InputActionReference.Create(realAction);

            // 刷新显示，让它显示当前的键位
            ui.UpdateBindingDisplay();
        }
        else
        {
            Debug.LogError($"在 InputManager 中找不到动作: {actionName}");
        }
    }
}