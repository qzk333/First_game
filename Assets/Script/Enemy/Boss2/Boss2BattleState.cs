using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2BattleState : EnemyState
{
    private Transform player;
    private Enemy_Boss2 enemy;
    private int moveDir;

    public Boss2BattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Boss2 _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.battleTime;

        if (PlayerManager.instance != null && PlayerManager.instance.player != null)
            player = PlayerManager.instance.player.transform;
    }

    public override void Update()
    {
        base.Update();

        if (player == null)
            return;

        RaycastHit2D playerDetected = enemy.IsPlayerDetected();
        if (playerDetected)
        {
            stateTimer = enemy.battleTime;
            if (enemy.CanDash())
            {
                stateMachine.ChangeState(enemy.dashState);
                return;
            }

            if (playerDetected.distance < enemy.attackDistance && CanAttack())
            {
                stateMachine.ChangeState(enemy.attackState);
                return;
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 10)
            {
                stateMachine.ChangeState(enemy.idleState);
                return;
            }
        }

        if (player.position.x > enemy.transform.position.x) moveDir = 1;
        else if (player.position.x < enemy.transform.position.x) moveDir = -1;

        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            enemy.SetVelocity(0, rb.velocity.y);
            stateMachine.ChangeState(enemy.idleState);
            return;
        }

        if (moveDir != enemy.facingDir)
            enemy.Flip();

        if (Mathf.Abs(player.position.x - enemy.transform.position.x) < 0.5f)
        {
            stateMachine.ChangeState(enemy.idleState);
            return;
        }

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    private bool CanAttack()
    {
        return Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown;
    }
}
