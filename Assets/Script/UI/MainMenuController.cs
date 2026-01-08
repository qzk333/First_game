using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio; //用于控制音量
using UnityEngine.SceneManagement; //用于跳转场景

public class MainMenuController : MonoBehaviour
{
    [Header("界面面板")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    [Header("设置组件")]
    public AudioMixer audioMixer; // 需要在Project里创建一个AudioMixer
    public Slider volumeSlider;

    void Start()
    {
        // 游戏开始时，确保主菜单显示，设置菜单隐藏
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);

        // 初始化音量条的值（如果有保存过的话）
        float savedVol;
        if (audioMixer != null && audioMixer.GetFloat("MasterVol", out savedVol))
        {
            // 这里涉及分贝转换，简单处理先略过，直接用Slider值
        }
    }

    // --- 按钮点击事件 ---

    // 开始新游戏
    public void OnStartGame()
    {
        // 创建新的游戏数据
        if (SaveManager.instance != null)
        {
            SaveManager.instance.NewGame();
        }
        
        // 加载游戏场景
        SceneManager.LoadScene(1);
    }
    
    // 继续游戏（读取存档）
    public void OnContinueGame()
    {
        // SaveManager会自动在场景加载时读取存档
        // 只需要切换场景即可
        SceneManager.LoadScene(1);
    }

    public void OnOpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OnCloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void OnQuitGame()
    {
        Application.Quit();
        Debug.Log("退出游戏"); // 编辑器里看不到退出，这行用于测试
    }

    // 音量控制逻辑
    public void SetVolume(float volume)
    {
        // 假设 audioMixer 暴露出的参数叫 "MasterVol"
        // Mathf.Log10 用于将线性滑条值(0.0001-1)转换为分贝(-80到0)
        if (audioMixer != null)
        {
            audioMixer.SetFloat("MasterVol", Mathf.Log10(volume) * 20);
        }
    }
}