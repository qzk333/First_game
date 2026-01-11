using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2IdleState : Boss2GroundedState
{
    public Boss2IdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Boss2 _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
        enemy.SetVelocity(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        stateTimer -= Time.deltaTime;

        if (enemy.IsPlayerDetected() && enemy.CanDash())
        {
            stateMachine.ChangeState(enemy.dashState);
            return;
        }

        float distanceToPlayer = player != null ? Vector2.Distance(enemy.transform.position, player.position) : Mathf.Infinity;

        if (distanceToPlayer < enemy.attackDistance && CanAttack())
        {
            stateMachine.ChangeState(enemy.attackState);
            return;
        }

        if (stateTimer < 0 && distanceToPlayer > 2f)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    private bool CanAttack()
    {
        return Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown;
    }
}
