using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangedAttackState : PlayerState
{
    public PlayerRangedAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.instance.PlaySFX(8, null, true);
        // 【滞空核心】
        // 瞬间停止所有移动
        player.SetVelocity(0, 0);
        // 关闭重力，让角色悬停在空中
        player.rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();
        
        // 恢复重力
        player.rb.gravityScale = player.defaultGravityScale;
    }

    public override void Update()
    {
        base.Update();
        
        // 持续锁定移动，防止滑动
        player.SetVelocity(0, 0);

        // 动画播完，切回 Idle (如果是地面) 或 Fall (如果是空中)
        if (triggerCalled)
        {
            if (player.IsGroundDetected())
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.fallState);
        }
    }
}
