using UnityEngine;

/// <summary>
/// RebindActionUI 修复脚本
/// 修复 RebindOverlay 为空时的错误
/// 将此脚本附加到每个 RebindUIPrefab 实例上
/// </summary>
public class RebindActionUIFix : MonoBehaviour
{
    private void Awake()
    {
        // 获取 RebindActionUI 组件
        var rebindActionUI = GetComponent("RebindActionUI");
        
        if (rebindActionUI != null)
        {
            // 使用反射设置 m_RebindOverlay 为 null，避免错误
            var overlayField = rebindActionUI.GetType().GetField("m_RebindOverlay", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (overlayField != null)
            {
                var currentValue = overlayField.GetValue(rebindActionUI);
                if (currentValue == null || currentValue.ToString() == "null")
                {
                    // 创建一个临时的空GameObject作为占位符
                    GameObject dummyOverlay = new GameObject("DummyOverlay");
                    dummyOverlay.transform.SetParent(transform);
                    dummyOverlay.SetActive(false);
                    overlayField.SetValue(rebindActionUI, dummyOverlay);
                }
            }
        }
    }
}
