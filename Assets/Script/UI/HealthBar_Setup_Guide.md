# 血条系统设置指南

## 📋 已创建的文件
- `Assets/Script/UI/HealthBarUI.cs` - 血条UI控制脚本

## 🎮 Unity中的设置步骤

### 方法一：使用 UI Slider（推荐）

1. **创建血条UI**
   - 在场景中右键 → UI → Slider
   - 这会自动创建 Canvas 和 Slider

2. **调整Slider设置**
   - 选择 Slider 对象
   - Inspector 中：
     - Interactable: ❌ 取消勾选（血条不需要交互）
     - Transition: None
     - Min Value: 0
     - Max Value: 100（会被脚本自动设置）

3. **设置血条为World Space**
   - 选择 Canvas
   - Render Mode: World Space
   - 调整 Canvas 的 Scale (建议 0.01, 0.01, 0.01)

4. **定位血条**
   - 将 Canvas 拖到角色对象下作为子对象
   - 调整位置到角色头顶 (例如 Position: 0, 2, 0)

5. **添加脚本**
   - 创建一个空物体作为 Canvas 的子对象，命名为 "HealthBar"
   - 将 `HealthBarUI.cs` 拖到这个物体上
   - 在 Inspector 中：
     - Health Slider: 拖入 Slider 对象
     - Fill Image: 拖入 Slider → Fill Area → Fill (会自动找到)

6. **调整血条外观**
   - 选择 Slider → Background：调整背景颜色（黑色或灰色）
   - 选择 Slider → Fill Area → Fill：调整填充颜色（会被脚本动态改变）
   - 删除 Slider → Handle Slide Area（血条不需要滑块）

### 方法二：简化版本

如果只想要快速测试，可以：

1. 创建 UI → Slider
2. 将 Slider 的 Canvas 设置为 World Space
3. 将 Canvas 作为角色的子对象
4. 添加 `HealthBarUI.cs` 到 Canvas 上
5. 脚本会自动查找 Slider 和 Fill Image

## ⚙️ HealthBarUI 组件设置

### Health Bar Settings（血条设置）
- **Health Slider**: Slider组件引用（可自动查找）
- **Fill Image**: 填充图片引用（可自动查找）

### Health Bar Colors（血条颜色）
- **Healthy Color**: 血量充足时的颜色（默认绿色，>60%）
- **Hurt Color**: 血量中等时的颜色（默认黄色，30-60%）
- **Critical Color**: 血量危险时的颜色（默认红色，<30%）

### Settings（其他设置）
- **Hide When Full**: 血满时隐藏血条
- **Face Camera**: 血条始终面向摄像机（World Space时很有用）

## 🎨 美化建议

1. **血条尺寸**
   - 在 Slider 的 Rect Transform 中调整 Width 和 Height
   - 推荐大小：Width: 100-200, Height: 10-20

2. **背景和边框**
   - 给 Slider 的 Background 添加 Outline 组件
   - 调整 Background 的颜色为半透明黑色

3. **平滑过渡**
   - 可以在代码中添加 Lerp 让血量变化更平滑
   - 目前是立即更新，如需平滑可以修改脚本

## 📝 工作原理

- **自动更新**: 血条每帧自动更新，无需手动调用
- **颜色变化**: 根据血量百分比自动改变颜色
- **父对象查找**: 脚本会自动在父对象中查找 CharacterStats 组件
- **摄像机朝向**: 如果启用，血条会始终面向主摄像机

## 🔧 常见问题

**Q: 血条不显示？**
- 检查 Canvas 是否在角色下作为子对象
- 检查是否有 CharacterStats 组件在父对象上
- 检查 Slider 组件是否正确引用

**Q: 血条颜色不变？**
- 确保 Fill Image 字段有引用
- 检查 Slider 的 Fill 对象是否有 Image 组件

**Q: 血条不面向摄像机？**
- 确保勾选了 Face Camera
- 确保场景中有标记为 MainCamera 的摄像机

## 💡 提示

- 可以为玩家和敌人分别设置不同的血条样式
- 可以添加血量数字显示（使用 TextMeshPro）
- 可以添加伤害数字飘出效果
- 建议使用 Prefab 来复用血条设置
