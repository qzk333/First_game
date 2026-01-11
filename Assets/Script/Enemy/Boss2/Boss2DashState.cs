using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2DashState : EnemyState
{
    private Enemy_Boss2 enemy;

    public Boss2DashState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Boss2 _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.ResetDashDamageFlag();
        Transform player = PlayerManager.instance != null && PlayerManager.instance.player != null
            ? PlayerManager.instance.player.transform
            : null;

        if (player != null)
        {
            int dir = player.position.x > enemy.transform.position.x ? 1 : -1;
            if (dir != enemy.facingDir)
                enemy.Flip();
        }

        stateTimer = enemy.dashDuration;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.RecordDash();
        enemy.SetVelocity(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.dashSpeed * enemy.facingDir, rb.velocity.y);

        if (!enemy.IsGroundDetected() || enemy.IsWallDetected())
        {
            enemy.SetVelocity(0, rb.velocity.y);
            enemy.Flip();
            stateMachine.ChangeState(enemy.moveState);
            return;
        }

        enemy.TryDealDashDamage();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.battleState);
    }
}
