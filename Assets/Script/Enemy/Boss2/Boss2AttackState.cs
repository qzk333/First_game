using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2AttackState : EnemyState
{
    private Enemy_Boss2 enemy;

    public Boss2AttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Boss2 _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.attackDuration;
        enemy.SetVelocity(0, 0);
    }

    public override void Exit()
    {
        base.Exit();
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
