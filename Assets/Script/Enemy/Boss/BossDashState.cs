using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDashState : BossState
{
    public BossDashState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Boss _boss) : base(_enemyBase, _stateMachine, _animBoolName, _boss)
    {
    }

    public override void Enter()
    {
        base.Enter();
        boss.lastDashTime = Time.time;
        stateTimer = boss.dashDuration;
        boss.stats.MakeInvincible(true); // Optional: Invincible during dash
    }

    public override void Exit()
    {
        base.Exit();
        boss.SetVelocity(0, rb.velocity.y);
        boss.stats.MakeInvincible(false);
    }

    public override void Update()
    {
        base.Update();

        boss.SetVelocity(boss.dashSpeed * boss.facingDir, rb.velocity.y);

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(boss.battleState);
        }
    }
}
