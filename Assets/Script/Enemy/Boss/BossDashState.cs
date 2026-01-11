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

        // Face the player before dashing
        Transform player = boss.GetPlayerTransform();
        if (player != null)
        {
            int dirToPlayer = (int)Mathf.Sign(player.position.x - boss.transform.position.x);
            if (dirToPlayer != 0 && dirToPlayer != boss.facingDir)
                boss.Flip();
        }

        AudioManager.instance.PlaySFX(16, null);
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

        // Abort dash if about to fall or hit wall: flip and go to move
        if (!boss.IsGroundDetected() || boss.IsWallDetected())
        {
            boss.SetVelocity(0, rb.velocity.y);
            boss.Flip();
            stateMachine.ChangeState(boss.moveState);
            return;
        }

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(boss.battleState);
        }
    }
}
