using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 下落状态 - 玩家在空中下落时进入此状态
/// 继承自 PlayerAirState，复用空中移动、双段跳、墙壁检测等逻辑
/// </summary>
public class PlayerFallState : PlayerAirState
{
    public PlayerFallState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update(); 

        // 检测是否落地，如果落地则切换到idle状态,如果落地且有x输入则切换到move状态
        if (player.IsGroundDetected()&&xInput==0)
            stateMachine.ChangeState(player.idleState);
        else if (player.IsGroundDetected()&&xInput!=0)
            stateMachine.ChangeState(player.moveState);
    }
}
