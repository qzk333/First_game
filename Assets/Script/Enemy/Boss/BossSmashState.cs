using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSmashState : BossState
{
    public BossSmashState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Boss _boss) : base(_enemyBase, _stateMachine, _animBoolName, _boss)
    {
    }

    public override void Enter()
    {
        base.Enter();
        boss.rb.gravityScale = 5; // Fall faster
    }

    public override void Exit()
    {
        base.Exit();
        boss.rb.gravityScale = 1; // Reset gravity
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
            stateMachine.ChangeState(boss.battleState);
    }
}
