using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionIdleState : MinionGroundedState
{
    public MinionIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Minion _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        stateTimer -= Time.deltaTime;
        
        // 检测玩家距离
        float distanceToPlayer = Vector2.Distance(enemy.transform.position, player.position);
        
        // 优先检查是否可以攻击
        if (distanceToPlayer < enemy.attackDistance && CanAttack())
        {
            stateMachine.ChangeState(enemy.attackState);
            return;
        }
        
        // 如果 idle 时间结束且玩家不在附近，切换到 move 状态
        if (stateTimer < 0 && distanceToPlayer > 2f)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }
    
    private bool CanAttack()
    {
        if(Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            return true;
        }
        return false;
    }
}
