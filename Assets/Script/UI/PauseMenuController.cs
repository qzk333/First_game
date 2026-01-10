using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [Header("暂停菜单面板")]
    [SerializeField] private GameObject pauseMenuPanel;
    
    private bool isPaused = false;
    
    private void Start()
    {
        // 确保开始时暂停菜单是隐藏的
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
    }
    
    private void Update()
    {
        // 使用新的 Input System 检测暂停键
        if (InputManager.instance == null || InputManager.instance.playerActions == null)
            return;

        if (InputManager.instance.playerActions.Player.Pause.WasPressedThisFrame())
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }
    
    // 暂停游戏
    public void PauseGame()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);
        
        Time.timeScale = 0f; // 暂停游戏时间
        isPaused = true;
    }
    
    // 继续游戏
    public void ResumeGame()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
        
        Time.timeScale = 1f; // 恢复游戏时间
        isPaused = false;
    }
    
    // 保存并继续
    public void SaveAndResume()
    {
        if (SaveManager.instance != null)
        {
            SaveManager.instance.SaveGame();
            Debug.Log("游戏已保存！");
        }
        ResumeGame();
    }
    
    // 保存并返回主菜单
    public void SaveAndReturnToMenu()
    {
        if (SaveManager.instance != null)
        {
            SaveManager.instance.SaveGame();
            Debug.Log("游戏已保存！");
        }
        
        Time.timeScale = 1f; // 恢复时间流速，否则主菜单会被冻结
        SceneManager.LoadScene(0); // 加载主菜单场景（假设索引为0）
    }
    
    // 不保存直接返回主菜单
    public void ReturnToMenuWithoutSaving()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    
    // 退出游戏
    public void QuitGame()
    {
        if (SaveManager.instance != null)
        {
            SaveManager.instance.SaveGame();
        }
        
        Application.Quit();
        Debug.Log("退出游戏");
    }
}
