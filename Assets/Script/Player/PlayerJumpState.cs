using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.instance.PlaySFX(4, null);

        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);

        player.ResetDoubleJump();
    }

    public override void Exit()
    {
        base.Exit();

        AudioManager.instance.StopSFX(4);
    }

    public override void Update()
    {
        base.Update();

   
        // 当Y轴速度为负（开始下落）时，切换到下落状态
        if (rb.velocity.y < 0)
            stateMachine.ChangeState(player.fallState);
     
    }
}
