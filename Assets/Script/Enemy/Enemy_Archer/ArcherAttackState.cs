using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAttackState : EnemyState
{
    private Enemy_Archer enemy;
    private Transform player;

    public ArcherAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetVelocity(0, 0);
        if (PlayerManager.instance != null && PlayerManager.instance.player != null)
            player = PlayerManager.instance.player.transform;

        if (player != null)
        {
            int dir = player.position.x > enemy.transform.position.x ? 1 : -1;
            if (dir != enemy.facingDir)
                enemy.Flip();
        }
    }

    public override void Exit()
    {
        base.Exit();
        stateTimer = enemy.attackDuration;
        enemy.lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(0, 0);

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.battleState);
    }
}
