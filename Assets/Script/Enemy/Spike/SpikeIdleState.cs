using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeIdleState : EnemyState
{
    private SpikeTrap enemy;

    public SpikeIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, SpikeTrap _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Update()
    {
        base.Update();

        // 如果玩家进入范围，立即切换到攻击状态
        if (enemy.IsPlayerDetected())
        {
            stateMachine.ChangeState(enemy.attackState);
        }
    }
}
