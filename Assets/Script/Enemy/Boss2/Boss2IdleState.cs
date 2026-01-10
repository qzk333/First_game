using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2IdleState : EnemyState
{
    private Enemy_Boss2 enemy;
    private Transform player;

    public Boss2IdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Boss2 _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetVelocity(0, 0); // 确保 Boss 处于静止
        if (PlayerManager.instance != null) player = PlayerManager.instance.player.transform;
    }

    public override void Update()
    {
        base.Update();

        // 核心逻辑：只有看到玩家才会进入战斗状态
        if (enemy.IsPlayerDetected())
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
