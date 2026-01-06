using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 按键重绑定管理器 - 提供按键绑定管理的辅助功能
/// 附加到设置面板的InputSet对象上
/// 注意：这个脚本不直接管理UI，UI需要使用Unity Sample中的RebindActionUI组件
/// </summary>
public class KeyRebindingManager : MonoBehaviour
{
    public static KeyRebindingManager instance { get; private set; }
    
    [Header("Audio Settings")]
    [Tooltip("重绑定开始时播放的音效")]
    public AudioClip rebindStartSound;
    
    [Tooltip("重绑定完成时播放的音效")]
    public AudioClip rebindCompleteSound;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    /// <summary>
    /// 重置所有按键为默认值
    /// 此方法应该绑定到UI按钮的OnClick事件
    /// </summary>
    public void ResetAllBindings()
    {
        if (InputManager.instance != null)
        {
            InputManager.instance.ResetAllBindings();
            
            // 播放音效
            if (rebindCompleteSound != null)
            {
                AudioSource.PlayClipAtPoint(rebindCompleteSound, Camera.main.transform.position);
            }
        }
    }

    /// <summary>
    /// 保存当前的按键绑定
    /// 此方法会自动在重绑定完成时调用，也可以手动调用
    /// </summary>
    public void SaveBindings()
    {
        if (InputManager.instance != null)
        {
            string bindings = InputManager.instance.SaveBindingOverrides();
            
            // 保存到 PlayerPrefs（在构建版本中有效）
            PlayerPrefs.SetString("InputBindingOverrides", bindings);
            PlayerPrefs.Save();
            
            // 直接更新并写入 GameData 文件（在 Unity Editor Play Mode 中也有效）
            string dataPath = Application.persistentDataPath + "/GameData.dat";
            GameData gameData;
            
            // 如果文件存在，读取现有数据
            if (System.IO.File.Exists(dataPath))
            {
                try
                {
                    string json = System.IO.File.ReadAllText(dataPath);
                    gameData = JsonUtility.FromJson<GameData>(json);
                }
                catch
                {
                    gameData = new GameData();
                }
            }
            else
            {
                gameData = new GameData();
            }
            
            // 更新绑定数据
            gameData.inputBindingOverrides = bindings;
            
            // 写入文件
            try
            {
                string json = JsonUtility.ToJson(gameData, true);
                System.IO.File.WriteAllText(dataPath, json);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"写入文件失败: {e.Message}");
            }
        }
    }

    /// <summary>
    /// 播放重绑定开始音效
    /// 可以从RebindActionUI的事件中调用
    /// </summary>
    public void PlayRebindStartSound()
    {
        if (rebindStartSound != null)
        {
            AudioSource.PlayClipAtPoint(rebindStartSound, Camera.main.transform.position);
        }
    }

    /// <summary>
    /// 播放重绑定完成音效并保存
    /// 可以从RebindActionUI的事件中调用
    /// </summary>
    public void PlayRebindCompleteSound()
    {
        if (rebindCompleteSound != null)
        {
            AudioSource.PlayClipAtPoint(rebindCompleteSound, Camera.main.transform.position);
        }

        if (InputManager.instance != null)
        {
            // 1. 获取最新的按键 JSON
            string json = InputManager.instance.SaveBindingOverrides();

            // 2. 【核心修复】通知 SaveManager 更新数据
            // 这样 SaveManager 的内存数据就和 InputManager 同步了，不会再发生覆盖旧数据的情况
            if (SaveManager.instance != null)
            {
                SaveManager.instance.SaveInputBindingOverrides(json);
            }
            else
            {
                // 如果在主菜单还没加载 SaveManager，才使用 PlayerPrefs 作为备用
                // 或者在这里手动处理文件，但通常 SaveManager 应该是全局单例
                PlayerPrefs.SetString("InputBindingOverrides", json);
                PlayerPrefs.Save();
            }

            // 3. 强制热重载 Input System (确保当前游戏状态立即生效)
            var actions = InputManager.instance.playerActions;
            actions.Disable();
            actions.LoadBindingOverridesFromJson(json);
            actions.Enable();

            Debug.Log($"✨ 按键修改完成！已同步至 SaveManager。");
        }
    }
}

