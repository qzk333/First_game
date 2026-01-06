# 远程攻击 (Sword Wave) 制作指南

代码已经全部写好了！现在你需要按照以下步骤来制作刀波的 Prefab 和配置 Player。

## 1. 制作刀波 (Sword Wave) Prefab
1. 在场景中创建一个新的空物体，命名为 `SwordWave`。
2. 给它添加 **Sprite Renderer** 组件，选一张像刀光一样的图片。
3. 给它添加 **Rigidbody 2D** 组件：
   - **Gravity Scale** 设为 `0` (非常重要，否则刀波会掉下去)。
   - **Collision Detection** 设为 `Continuous` (防止穿墙)。
4. 给它添加 **Box Collider 2D** 组件：
   - 勾选 **Is Trigger**。
   - 调整大小以匹配刀光图片。
5. 给它添加代码脚本 **`SwordWaveController`** (直接拖上去)。
   - Speed: 设为 10 或 15。
   - Life Time: 默认 3 秒即可。
6. **保存为 Prefab**：把做好的物体拖进 Project 面板的 Prefab 文件夹，然后把场景里的删掉。

## 2. 配置 Animator (Player)
1. 打开 Player 的 Animator。
2. 添加 **Bool** 参数：**`RangedAttack`**。
3. 新建状态 **`RangedAttack`** (用攻击动画)。
4. 连线：`Any State` -> `RangedAttack`。
   - Condition: `RangedAttack` is `true`。
   - 取消 Have Exit Time。
5. 连线：`RangedAttack` -> `Fall` (或 Idle)。
   - Condition: `RangedAttack` is `false` (可选).
   - 取消 Have Exit Time。

## 3. 添加动画事件 (Animation Event)
1. 打开刀波攻击的 Animation Clip。
2. 在**挥刀的那一帧**，添加事件：
   - Function: **`RangedAttackTrigger`**
3. 在**最后一帧**，添加事件：
   - Function: **`AnimationTrigger`**

## 4. 配置 Player 脚本
1. 选中 Player。
2. 在 Inspector 找到脚本的 **Ranged Attack** 区域。
3. 把刚才做好的 **Sword Wave Prefab** 拖进 **`Sword Wave Prefab`** 槽位。
4. **按键**：目前代码绑定的是键盘 **`K`** 键（在地面或空中按 K）。

---
完成后，无论是在地面还是跳跃时按 K，你都能发射出一道刀光！空中使用时还会悬停哦！
