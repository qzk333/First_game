# 暂停菜单设置指南

## 📋 已创建的文件
- `PauseMenuController.cs` - 暂停菜单控制脚本

## 🎮 Unity中的设置步骤

### 第一步：创建暂停菜单UI

1. **在游戏场景中创建Canvas**（如果还没有）
   - 右键场景 → UI → Canvas
   - Canvas 设置：
     - Render Mode: Screen Space - Overlay
     - 确保有 EventSystem

2. **创建暂停菜单面板**
   - 选择 Canvas
   - 右键 → UI → Panel
   - 重命名为 "PauseMenuPanel"
   - 这个Panel会作为半透明的背景遮罩

3. **调整Panel外观**
   - 选择 PauseMenuPanel
   - Image 组件：
     - Color: 黑色，Alpha: 150-200（半透明）
   - Rect Transform:
     - Anchor Presets: 拉伸到全屏（Alt+Shift + 右下角）

### 第二步：创建菜单内容

在 PauseMenuPanel 下创建菜单内容：

1. **创建标题**
   - 右键 PauseMenuPanel → UI → Text - TextMeshPro
   - 命名为 "Title"
   - 文字: "游戏暂停" 或 "PAUSED"
   - 字体大小: 48-60
   - 对齐: 居中
   - 颜色: 白色

2. **创建按钮组**
   - 右键 PauseMenuPanel → UI → Vertical Layout Group
   - 命名为 "ButtonGroup"
   - 设置 Vertical Layout Group：
     - Spacing: 10-20
     - Child Alignment: Middle Center
     - Child Force Expand: Width ✅

3. **创建按钮**（在ButtonGroup下）

   **继续游戏按钮**
   - UI → Button - TextMeshPro
   - 重命名: "ResumeButton"
   - 文字: "继续游戏"
   
   **保存游戏按钮**
   - UI → Button - TextMeshPro
   - 重命名: "SaveButton"
   - 文字: "保存游戏"
   
   **返回主菜单按钮**
   - UI → Button - TextMeshPro
   - 重命名: "MainMenuButton"
   - 文字: "返回主菜单"
   
   **退出游戏按钮**
   - UI → Button - TextMeshPro
   - 重命名: "QuitButton"
   - 文字: "退出游戏"

### 第三步：添加脚本

1. **创建控制器对象**
   - 在Canvas下创建空物体
   - 命名为 "PauseMenuController"
   - 添加 PauseMenuController.cs 脚本

2. **配置脚本**
   - 选择 PauseMenuController 对象
   - 在 Inspector 中：
     - Pause Menu Panel: 拖入 PauseMenuPanel
     - Pause Key: Escape（默认，可改为其他键）

### 第四步：设置按钮事件

为每个按钮设置点击事件：

1. **继续游戏按钮**
   - 选择 ResumeButton
   - Button 组件 → OnClick()
   - 点击 + 号
   - 拖入 PauseMenuController 对象
   - 函数选择: PauseMenuController → **ResumeGame()**

2. **保存游戏按钮**
   - 选择 SaveButton
   - OnClick() → PauseMenuController → **SaveAndResume()**

3. **返回主菜单按钮**
   - 选择 MainMenuButton
   - OnClick() → PauseMenuController → **SaveAndReturnToMenu()**
   - 或者 **ReturnToMenuWithoutSaving()** 如果不想保存

4. **退出游戏按钮**
   - 选择 QuitButton
   - OnClick() → PauseMenuController → **QuitGame()**

### 第五步：测试

1. 运行游戏
2. 按 **ESC** 键，应该会显示暂停菜单
3. 再按 **ESC** 或点击"继续游戏"，菜单消失
4. 测试所有按钮功能

## 🎨 美化建议

### 按钮样式
```
- 正常颜色: 深色背景
- 高亮颜色: 稍亮的颜色
- 按下颜色: 更深的颜色
- 禁用颜色: 灰色
```

### 添加按钮音效
在每个按钮的 Button 组件下可以添加 Audio Source

### 添加动画
可以给 PauseMenuPanel 添加淡入淡出动画：
1. 创建 Animator Controller
2. 添加 Fade In/Out 动画
3. 在脚本中触发动画而不是直接 SetActive

## 📝 脚本功能说明

### 可用方法

| 方法 | 功能 | 是否保存 |
|------|------|---------|
| `PauseGame()` | 暂停游戏，显示菜单 | - |
| `ResumeGame()` | 继续游戏，隐藏菜单 | - |
| `SaveAndResume()` | 保存并继续 | ✅ |
| `SaveAndReturnToMenu()` | 保存并返回主菜单 | ✅ |
| `ReturnToMenuWithoutSaving()` | 直接返回主菜单 | ❌ |
| `QuitGame()` | 保存并退出游戏 | ✅ |

### 关键设置

- **暂停键**: 默认 ESC，可在Inspector中修改
- **Time.timeScale**: 
  - 0 = 游戏暂停
  - 1 = 正常速度
  - 2 = 2倍速（可用于调试）

## ⚠️ 注意事项

1. **Time.timeScale 的影响**
   - 暂停时 `Time.deltaTime` 会变成 0
   - 使用 `Time.unscaledDeltaTime` 的系统不受影响（如UI动画）

2. **场景索引**
   - 主菜单场景索引默认为 0
   - 如果不同，需要在脚本中修改 `SceneManager.LoadScene(0)`

3. **保存时机**
   - 返回主菜单前会自动保存
   - 退出游戏前会自动保存
   - 也可以手动点击"保存游戏"按钮

4. **ESC键冲突**
   - 如果游戏中有其他使用ESC的功能，需要调整逻辑
   - 可以改用其他键如 P、Tab 等

## 🎯 推荐布局

```
Canvas
├── PlayerHealthBar (之前创建的)
└── PauseMenuPanel
    ├── Title (Text)
    └── ButtonGroup (Vertical Layout)
        ├── ResumeButton
        ├── SaveButton
        ├── MainMenuButton
        └── QuitButton
```

## 🔧 进阶功能

### 1. 添加音量设置
可以在暂停菜单中添加设置界面：
- 复用主菜单的设置面板
- 添加音量滑条
- 添加画质选项等

### 2. 添加确认对话框
退出前显示确认对话框：
```csharp
public void ShowQuitConfirmation()
{
    // 显示确认对话框
    confirmDialog.SetActive(true);
}
```

### 3. 键位提示
在暂停菜单底部显示：
"按 ESC 继续游戏"

### 4. 统计信息
显示当前游戏时间、击杀数等

## 💡 使用技巧

### 快捷键
- **ESC** - 暂停/继续
- **F5** - 快速保存（可在脚本中添加）
- **F9** - 快速读档（可在脚本中添加）

### 调试模式
在开发时可以添加：
```csharp
if (Input.GetKeyDown(KeyCode.T))
{
    Time.timeScale = 2f; // 2倍速测试
}
```
