using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionAttackState : EnemyState
{
    private Enemy_Minion enemy;

    public MinionAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Minion _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        stateTimer=enemy.attackDuration;
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
