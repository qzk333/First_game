using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoveState : BossState
{
    public BossMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Boss _boss) : base(_enemyBase, _stateMachine, _animBoolName, _boss)
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

        boss.SetVelocity(boss.moveSpeed * boss.facingDir, rb.velocity.y);

        Transform player = boss.GetPlayerTransform();
        bool playerClose = player != null && Vector2.Distance(boss.transform.position, player.position) < 10; // Basic detection check

        if (boss.IsPlayerDetected() || playerClose)
        {
            stateMachine.ChangeState(boss.battleState);
        }
    }
}
