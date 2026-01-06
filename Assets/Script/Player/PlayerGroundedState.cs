using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.ResetDoubleJump();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (InputManager.instance != null)
        {
            if (InputManager.instance.playerActions.Player.CounterAttack.WasPressedThisFrame())
                stateMachine.ChangeState(player.counterAttack);

            if (InputManager.instance.playerActions.Player.Attack.WasPressedThisFrame())
                stateMachine.ChangeState(player.primaryAttack);

            if (InputManager.instance.playerActions.Player.Jump.WasPressedThisFrame() && player.IsGroundDetected())
                stateMachine.ChangeState(player.jumpState);
        }

        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.fallState);
    }
}
