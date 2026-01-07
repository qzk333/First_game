using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

        // 双段跳逻辑 - 在空中按空格可以进行二段跳
        if (InputManager.instance != null && 
            InputManager.instance.playerActions.Player.Jump.WasPressedThisFrame() && 
            !player.IsGroundDetected())
        {
            player.PerformDoubleJump();
        }

        // 墙壁检测 - 碰到墙壁切换到墙滑状态
        if (player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlide);

        // 远程攻击检测 (暂时绑定为 K 键，你可以改成自己想要的)
        if (InputManager.instance.playerActions.Player.RangedAttack.WasPressedThisFrame())
        {
            if (player.stats.HasEnoughRage(3))
            {
                player.stats.DecreaseRage(3);
                stateMachine.ChangeState(player.rangedAttackState);
            }
            else
            {
                Debug.Log("Not enough Rage!");
            }
            return;
        }

        // 空中攻击检测
        if (InputManager.instance != null && InputManager.instance.playerActions.Player.Attack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.jumpAttackState);
            return; // 切换状态后直接返回，避免执行后续的移动逻辑(可选)
        }

        // 空中移动 - 在空中可以左右移动，速度为地面的80%
        if (xInput != 0)
            player.SetVelocity(player.moveSpeed * .8f * xInput, rb.velocity.y);
    }
}
