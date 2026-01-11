using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeadState : BossState
{
    public BossDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Boss _boss) : base(_enemyBase, _stateMachine, _animBoolName, _boss)
    {
    }

    public override void Enter()
    {
        base.Enter();
        boss.cd.enabled = false;
        boss.rb.gravityScale = 0;
        AudioManager.instance.PlaySFX(6, null);
    }

    public override void Exit()
    {
        base.Exit();

        AudioManager.instance.StopSFX(6);
    }

    public override void Update()
    {
        base.Update();
        boss.SetVelocity(0, 0);
    }
}
