using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 输入管理器 - 管理玩家输入动作和按键绑定
/// </summary>
public class InputManager : MonoBehaviour
{
    public static InputManager instance { get; private set; }

    public PlayerInputActions playerActions { get; private set; }

    private void Awake()
    {
        // 单例模式
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // 初始化输入动作
        playerActions = new PlayerInputActions();
        
        // 加载保存的绑定覆盖
        LoadBindingOverrides();
        
        // 启用输入系统
        playerActions.Enable();
    }

    private void OnEnable()
    {
        // OnEnable 可能在 Awake 之前调用，所以移到 Awake 末尾
        // if (playerActions != null)
        // {
        //     playerActions.Enable();
        // }
    }

    private void OnDisable()
    {
        if (playerActions != null)
        {
            playerActions.Disable();
        }
    }
    /// <summary>
    /// 保存当前的按键绑定覆盖
    /// </summary>
    public string SaveBindingOverrides()
    {
        return playerActions.SaveBindingOverridesAsJson();
    }

    /// <summary>
    /// 加载按键绑定覆盖
    /// </summary>
    public void LoadBindingOverrides(string overrides = null)
    {
        if (string.IsNullOrEmpty(overrides))
        {
            // 尝试直接从存档文件读取
            string dataPath = Application.persistentDataPath + "/GameData.dat";
            
            if (System.IO.File.Exists(dataPath))
            {
                try
                {
                    string json = System.IO.File.ReadAllText(dataPath);
                    GameData gameData = JsonUtility.FromJson<GameData>(json);
                    
                    if (gameData != null && !string.IsNullOrEmpty(gameData.inputBindingOverrides))
                    {
                        overrides = gameData.inputBindingOverrides;
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"读取存档文件失败: {e.Message}");
                }
            }
            
            // 如果文件中没有，再尝试从 PlayerPrefs 加载
            if (string.IsNullOrEmpty(overrides))
            {
                overrides = PlayerPrefs.GetString("InputBindingOverrides", string.Empty);
            }
        }

        if (!string.IsNullOrEmpty(overrides))
        {
            playerActions.LoadBindingOverridesFromJson(overrides);
        }
    }

    /// <summary>
    /// 重置所有绑定为默认值
    /// </summary>
    public void ResetAllBindings()
    {
        foreach (var map in playerActions.asset.actionMaps)
        {
            map.RemoveAllBindingOverrides();
        }
        
        // 清除保存的覆盖
        PlayerPrefs.DeleteKey("InputBindingOverrides");
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 重置特定动作的绑定
    /// </summary>
    public void ResetBinding(InputAction action, int bindingIndex)
    {
        if (action != null && bindingIndex >= 0 && bindingIndex < action.bindings.Count)
        {
            action.RemoveBindingOverride(bindingIndex);
        }
    }
}
