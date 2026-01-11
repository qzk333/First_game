using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2ChargeState : EnemyState
{
    private Enemy_Boss2 enemy;

    public Boss2ChargeState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Boss2 _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.chargeDuration; // 使用主脚本定义的持续时间

        AudioManager.instance.PlaySFX(10, null);
    }

    public override void Update()
    {
        base.Update();

        // 使用自定义的冲锋速度（建议在 Enemy_Boss2 中定义，这里先写死一个数值）
        enemy.SetVelocity(enemy.chargeSpeed * enemy.facingDir, rb.velocity.y);

        // 如果撞墙或时间到，结束冲锋
        if (enemy.IsWallDetected() || stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeCharged = Time.time; // 退出时记录冷却时间
        enemy.lastTimeAttacked = Time.time;  //冲刺攻击
        AudioManager.instance.StopSFX(10);
    }
}
