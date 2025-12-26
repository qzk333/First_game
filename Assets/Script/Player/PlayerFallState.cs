using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        if (Input.GetKeyDown(KeyCode.Space) && !player.IsGroundDetected())
        {
            player.PerformDoubleJump();
            return; // 执行二段跳后直接返回，避免状态切换冲突
        }


        if (player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlide);
        //做了点修改，如果落地速度为0，则静止，不为0则直接移动
        if (player.IsGroundDetected() && player.rb.velocity.x == 0)
            stateMachine.ChangeState(player.idleState);

        if (player.IsGroundDetected() && player.rb.velocity.x != 0)
            stateMachine.ChangeState(player.moveState);

        if (xInput != 0)
            player.SetVelocity(player.moveSpeed * .8f * xInput, rb.velocity.y);
    }
}
