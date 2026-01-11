using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBattleState : EnemyState
{
    private Transform player;
    private Enemy_Archer enemy;

    public ArcherBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        if (PlayerManager.instance != null && PlayerManager.instance.player != null)
            player = PlayerManager.instance.player.transform;

        stateTimer = enemy.battleTime;
    }

    public override void Update()
    {
        base.Update();

        if (player == null)
            return;

        enemy.SetVelocity(0, rb.velocity.y);

        if (enemy.IsPlayerDetected())
            stateTimer = enemy.battleTime;
        else if (stateTimer < 0 || Vector2.Distance(player.position, enemy.transform.position) > 10f)
            stateMachine.ChangeState(enemy.idleState);

        int moveDir = player.position.x > enemy.transform.position.x ? 1 : -1;
        if (moveDir != enemy.facingDir)
            enemy.Flip();

        if (CanAttack())
            stateMachine.ChangeState(enemy.attackState);
    }

    private bool CanAttack()
    {
        return Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown;
    }
}
