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

            // 远程攻击检测 (暂时绑定为 K 键)
            if (InputManager.instance.playerActions.Player.RangedAttack.WasPressedThisFrame())
            {
                // 检查远程攻击是否解锁
                if (!player.unlockRangedAttack)
                {
                    // 可以选择提示玩家技能未解锁，或者什么都不做
                    Debug.Log("Ranged Attack is locked!");
                }
                // 检查怒气
                else if (player.stats.HasEnoughRage(3))
                {
                    player.stats.DecreaseRage(3);
                    stateMachine.ChangeState(player.rangedAttackState);
                }
                else
                {
                    Debug.Log("Not enough Rage for Ranged Attack!");
                }
            }

            // 回血检测 (需在 Input Actions 添加 Heal 绑定)
            if (InputManager.instance.playerActions.Player.Heal.WasPressedThisFrame())
            {
                // 能够进入回血状态的前提是至少有3点怒气，否则不让进
                if (player.stats.HasEnoughRage(3))
                {
                    stateMachine.ChangeState(player.healState);
                }
                else
                {
                   Debug.Log("Not enough Rage to start healing!");
                }
            }
        }

        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.fallState);
    }
}
