using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealState : PlayerState
{
    public PlayerHealState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // 停止移动
        player.SetVelocity(0, 0);
        
        // 设置回血所需时间 (从 Player 读取配置)
        stateTimer = player.healDuration;
    }

    public override void Update()
    {
        base.Update();

        // 持续强制静止
        player.SetVelocity(0, 0);

        // 如果按住 H 键 (这里暂时固定为 H，或者读取 InputManager)
        // 也可以用 Input.GetKey(KeyCode.H)
        // 检查按键是否持续按住
        if (InputManager.instance.playerActions.Player.Heal.IsPressed())
        {
            // 如果蓄力时间到了
            if (stateTimer < 0)
            {
                // 执行回血 (再次检查怒气，确保蓄力期间没有被扣除虽然不太可能)
                if (player.stats.HasEnoughRage(3))
                {
                    player.stats.DecreaseRage(3);

                    int healAmount = (int)(player.stats.maxHealth.GetValue() * player.healPercent);
                    player.stats.currentHealth += healAmount;
                
                  // 限制不超过最大血量
                  if (player.stats.currentHealth > player.stats.maxHealth.GetValue())
                      player.stats.currentHealth = player.stats.maxHealth.GetValue();
                  
                  Debug.Log($"Healed! Current Health: {player.stats.currentHealth}, Rage Left: {player.stats.currentRage}");
                }
                
                // 回完一次切回 Idle
                stateMachine.ChangeState(player.idleState);
            }
        }
        else
        {
            // 如果中途松开按键，聚集失败，切回 Idle
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        // 可以在这里停止回血特效
    }
}
