using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleState : BossState
{
    private Transform player;
    private int moveDir;

    public BossBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Boss _boss) : base(_enemyBase, _stateMachine, _animBoolName, _boss)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player = boss.GetPlayerTransform();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player == null)
        {
            player = boss.GetPlayerTransform();
            if (player == null)
            {
                stateMachine.ChangeState(boss.idleState);
                return;
            }
        }

        if (boss.IsPlayerDetected())
        {
            stateTimer = boss.battleTime;

            if (boss.IsPlayerDetected().distance < boss.attackDistance)
            {
                if (CanAttack())
                {
                    stateMachine.ChangeState(boss.attackState);
                    return;
                }
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, boss.transform.position) > 15)
            {
                stateMachine.ChangeState(boss.idleState);
                return;
            }
        }

        // Skill Decision Logic
        // 1. Teleport if cooldown ready and far away or random chance
        if (boss.CanTeleport() && Random.value > 0.7f) // % chance to consider
        {
            stateMachine.ChangeState(boss.teleportState);
            return;
        }

        // 2. Dash if cooldown ready and distance is medium
        float distanceToPlayer = Vector2.Distance(boss.transform.position, player.position);
        if (boss.CanDash() && distanceToPlayer > 5 && distanceToPlayer < 12)
        {
            stateMachine.ChangeState(boss.dashState);
            return;
        }

        // Movement Logic in Battle
        if (player.position.x > boss.transform.position.x)
            moveDir = 1;
        else if (player.position.x < boss.transform.position.x)
            moveDir = -1;

        boss.SetVelocity(boss.moveSpeed * moveDir * 1.5f, rb.velocity.y); // Move faster in battle
    }

    private bool CanAttack()
    {
        if (Time.time >= boss.lastTimeAttacked + boss.attackCooldown)
        {
            boss.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }
}
