# 存档系统使用指南

## ✅ 已完成的修改

### 1. SaveManager.cs
- ✅ 添加了 `DontDestroyOnLoad` - SaveManager在场景切换时不会被销毁
- ✅ 添加了场景加载事件 - 每次加载新场景时自动重新查找ISaveManager对象并加载数据
- ✅ 修复了单例模式的bug

### 2. GameData.cs
- ✅ 添加了玩家数据字段: `playerHealth`, `playerMaxHealth`, `playerPosition`
- ✅ 创建了 `SerializableVector3` 类用于保存位置
- ✅ 可以继续添加更多数据字段（如等级、技能等）

### 3. MainMenuController.cs
- ✅ `OnStartGame()` - 开始新游戏（创建新存档）
- ✅ `OnContinueGame()` - 继续游戏（读取存档）

## 🎮 Unity中的设置步骤

### 第一步：设置SaveManager

1. **创建SaveManager对象**（在主菜单场景）
   - 创建空物体，命名为 "SaveManager"
   - 添加 SaveManager 脚本
   - 在Inspector中设置:
     - File Name: "gamedata.json" (或任何你想要的名字)
     - Initialize Data If Null: ✅ 勾选（可选）

### 第二步：确保玩家有Tag

1. 选择玩家对象
2. Inspector顶部设置 Tag 为 **"Player"**

### 第三步：让玩家实现存档接口

如果还没有，需要修改 PlayerManager.cs 使其实现保存/加载玩家数据：

```csharp
public void SaveData(ref GameData _data)
{
    // 保存玩家数据
    _data.playerHealth = player.stats.currentHealth;
    _data.playerMaxHealth = player.stats.maxHealth.GetValue();
    _data.playerPosition = SerializableVector3.FromVector3(player.transform.position);
}

public void LoadData(GameData _data)
{
    // 加载玩家数据
    player.stats.currentHealth = _data.playerHealth;
    player.transform.position = _data.playerPosition.ToVector3();
}
```

### 第四步：设置按钮

在主菜单中，修改按钮的点击事件：

1. **"开始游戏"按钮**
   - OnClick() → MainMenuController.OnStartGame()

2. **"读取设置"按钮**（改为"继续游戏"）
   - OnClick() → MainMenuController.OnContinueGame()

3. **或者创建两个按钮**
   - "新游戏" → OnStartGame()
   - "继续游戏" → OnContinueGame()

## 📝 工作流程

### 新游戏流程
1. 玩家点击"开始游戏"
2. `OnStartGame()` 调用 `SaveManager.NewGame()` 创建新的GameData
3. 切换到游戏场景
4. `OnSceneLoaded` 触发，重新查找ISaveManager对象
5. `LoadGame()` 加载数据（此时是新的干净数据）

### 继续游戏流程
1. 玩家点击"继续游戏"
2. 切换到游戏场景
3. `OnSceneLoaded` 触发
4. `LoadGame()` 从文件读取存档并应用到游戏

### 保存流程
- 游戏退出时自动保存（`OnApplicationQuit`）
- 也可以在任何时候调用 `SaveManager.instance.SaveGame()`

## 🔧 如何添加更多数据

### 在 GameData.cs 中添加字段

```csharp
public class GameData
{
    public int currency;
    public int playerHealth;
    // ... 现有字段

    // 新增字段示例:
    public int playerLevel;
    public int playerExperience;
    public List<string> unlockedSkills;
    public Dictionary<string, bool> completedQuests; // 注意：Dictionary需要特殊处理

    public GameData()
    {
        // 在构造函数中初始化
        this.playerLevel = 1;
        this.playerExperience = 0;
        this.unlockedSkills = new List<string>();
    }
}
```

### 在 ISaveManager 实现中保存/加载

```csharp
public void SaveData(ref GameData _data)
{
    _data.playerLevel = myLevel;
    _data.playerExperience = myExperience;
    _data.unlockedSkills = new List<string>(myUnlockedSkills);
}

public void LoadData(GameData _data)
{
    myLevel = _data.playerLevel;
    myExperience = _data.playerExperience;
    myUnlockedSkills = new List<string>(_data.unlockedSkills);
}
```

## 💾 存档文件位置

存档文件保存在：
- **Windows**: `C:/Users/[用户名]/AppData/LocalLow/[公司名]/[游戏名]/gamedata.json`
- **Mac**: `~/Library/Application Support/[公司名]/[游戏名]/gamedata.json`
- **Android**: 内部存储

可以使用 `Application.persistentDataPath` 查看确切路径（在SaveManager的Start中添加Debug.Log）

## 🐛 调试技巧

### 查看存档路径
```csharp
Debug.Log("Save file path: " + Application.persistentDataPath);
```

### 强制保存测试
在Unity编辑器中添加快捷键：
```csharp
private void Update()
{
    if (Input.GetKeyDown(KeyCode.F5))
    {
        SaveManager.instance.SaveGame();
        Debug.Log("Game saved!");
    }
    
    if (Input.GetKeyDown(KeyCode.F9))
    {
        SaveManager.instance.LoadGame();
        Debug.Log("Game loaded!");
    }
}
```

### 删除存档测试
直接删除上述路径中的 gamedata.json 文件

## ⚠️ 注意事项

1. **场景顺序**
   - 确保场景在 Build Settings 中正确添加
   - 主菜单场景 = 0，游戏场景 = 1

2. **SaveManager位置**
   - SaveManager必须在主菜单场景中创建
   - 它会自动在场景切换时保持存在（DontDestroyOnLoad）

3. **ISaveManager实现**
   - 所有需要保存数据的类都要实现 ISaveManager 接口
   - 确保在 SaveData 和 LoadData 中正确处理数据

4. **序列化限制**
   - Unity的 JsonUtility 不支持 Dictionary
   - 需要将 Dictionary 转换为 List 再保存
   - Vector3、Quaternion等需要创建可序列化版本

## 🎯 推荐的按钮布局

建议在主菜单中设置：
- **新游戏** (OnStartGame) - 创建新存档开始
- **继续游戏** (OnContinueGame) - 加载上次存档
- **设置** (OnOpenSettings)
- **退出** (OnQuitGame)

如果想要在"继续游戏"按钮上显示"无存档"状态，可以在Start中检查：
```csharp
FileDataHandler handler = new FileDataHandler(Application.persistentDataPath, "gamedata.json");
GameData data = handler.Load();
continueButton.interactable = (data != null); // 没有存档时禁用按钮
```
