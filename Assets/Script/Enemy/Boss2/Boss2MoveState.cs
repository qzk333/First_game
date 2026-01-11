using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2MoveState : Boss2GroundedState
{
    public Boss2MoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Boss2 _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.velocity.y);

        if (enemy.IsPlayerDetected() && enemy.CanDash())
        {
            stateMachine.ChangeState(enemy.dashState);
            return;
        }

        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
