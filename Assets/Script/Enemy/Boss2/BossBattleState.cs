using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleState : EnemyState
{
    private Enemy_Boss enemy;
    private Transform player;

    public BossBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Boss _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        if (PlayerManager.instance != null) player = PlayerManager.instance.player.transform;
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            // 距离够了就攻击
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack()) stateMachine.ChangeState(enemy.attackState);
                return;
            }
        }
        else
        {
            // 失去目标回到 Idle 重新变回静止
            stateMachine.ChangeState(enemy.idleState);
            return;
        }

        // 朝向玩家移动
        float moveDir = player.position.x > enemy.transform.position.x ? 1 : -1;
        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    private bool CanAttack() => Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown;
}
