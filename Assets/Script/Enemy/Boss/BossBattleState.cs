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

        AudioManager.instance.PlaySFX(15, null);
        player = boss.GetPlayerTransform();
    }

    public override void Exit()
    {
        base.Exit();
        //AudioManager.instance.StopSFX(15);
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

        float distanceToPlayer = Vector2.Distance(boss.transform.position, player.position);

        if (boss.IsPlayerDetected() || distanceToPlayer < 8f)
        {
            stateTimer = boss.battleTime;

            if (distanceToPlayer < boss.attackDistance)
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
                boss.SetVelocity(0, rb.velocity.y);
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
        if (boss.CanDash() && distanceToPlayer > 5 && distanceToPlayer < 12)
        {
            stateMachine.ChangeState(boss.dashState);
            return;
        }

        // Handle walls / ledges: flip and keep moving instead of stopping
        if (boss.IsWallDetected() || !boss.IsGroundDetected())
        {
            boss.Flip();
        }

        // Movement Logic in Battle
        if (player.position.x > boss.transform.position.x)
            moveDir = 1;
        else if (player.position.x < boss.transform.position.x)
            moveDir = -1;

        // Too close: stay in battle but停下
        if (distanceToPlayer < boss.attackDistance)
        {
            boss.SetVelocity(0, rb.velocity.y);
            return;
        }

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
