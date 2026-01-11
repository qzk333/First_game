using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : BossState
{
    public BossIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Boss _boss) : base(_enemyBase, _stateMachine, _animBoolName, _boss)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = boss.idleTime;
        boss.SetVelocity(0, 0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // React to player immediately (even before idle timer finishes)
        Transform player = boss.GetPlayerTransform();
        if (boss.IsPlayerDetected() || (player != null && Vector2.Distance(boss.transform.position, player.position) < 8f))
        {
            stateMachine.ChangeState(boss.battleState);
            return;
        }

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(boss.moveState);
        }
    }
}
