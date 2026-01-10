using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName;

    private GameData gameData;
    private List<ISaveManager> saveManagers;
    private FileDataHandler dataHandler;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
            saveManagers = FindAllSaveManagers();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Prevent action if not the singleton instance (double safety)
        if (instance != this) return;

        // 每次加载新场景时，重新查找所有ISaveManager
        this.saveManagers = FindAllSaveManagers();
        LoadGame();
    }

    private void Start()
    {
        LoadGame();
    }

    public void NewGame()
    {
        // Preserve input bindings if they exist
        string savedBindings = (gameData != null) ? gameData.inputBindingOverrides : string.Empty;

        // Create fresh data
        gameData = new GameData();

        // Restore bindings
        gameData.inputBindingOverrides = savedBindings;

        // Save immediately to disk so OnSceneLoaded reads this valid new state
        dataHandler.Save(gameData);
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();
        
        if (this.gameData == null)
        {
            Debug.Log("No saved data found!");
            NewGame();
        }

        // 加载输入绑定覆盖
        if (InputManager.instance != null && !string.IsNullOrEmpty(gameData.inputBindingOverrides))
        {
            InputManager.instance.LoadBindingOverrides(gameData.inputBindingOverrides);
        }

        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        // 保存输入绑定覆盖
        if (InputManager.instance != null)
        {
            gameData.inputBindingOverrides = InputManager.instance.SaveBindingOverrides();
        }

        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }

        // Mark that the game has now been saved at least once
        gameData.isFirstLoad = false;

        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();

        return new List<ISaveManager>(saveManagers);
    }

    public void SaveInputBindingOverrides(string overridesJson)
    {
        // 1. 确保 gameData 已经初始化
        if (gameData == null)
        {
            gameData = new GameData();
        }

        // 2. 更新内存中的数据 (这是最关键的一步！防止脏数据覆盖)
        gameData.inputBindingOverrides = overridesJson;

        // 3. 立即写入硬盘
        dataHandler.Save(gameData);

        Debug.Log("SaveManager: 已更新内存并保存按键设置到 GameData.dat");
    }
}
