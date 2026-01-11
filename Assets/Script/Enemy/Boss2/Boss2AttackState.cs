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
        enemy.attackCounter++; // 每次进入攻击状态，计数加 1
        AudioManager.instance.PlaySFX(enemy.attackCounter + 11, null, true);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;

        // 如果三连击完成，重置计数
        if (enemy.attackCounter >= 3)
            enemy.attackCounter = 0;
    }

    public override void Update()
    {
        base.Update();
        enemy.SetVelocity(0, 0);

        // 动画结束触发器（在 AnimationTriggers 中调用）
        if (triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }
}
