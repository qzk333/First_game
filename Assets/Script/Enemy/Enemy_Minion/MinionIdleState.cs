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
        // 调用 EnemyState.Update() 来更新 stateTimer，但不调用 MinionGroundedState.Update()
        // 这样避免使用 grounded 的状态转换逻辑
        stateTimer -= Time.deltaTime;
        
        // 检测玩家
        Transform player = null;
        if (PlayerManager.instance != null && PlayerManager.instance.player != null)
            player = PlayerManager.instance.player.transform;
            
        if (player == null)
            return;
        
        // 检测玩家距离
        float distanceToPlayer = Vector2.Distance(enemy.transform.position, player.position);
        
        // 优先检查是否可以攻击
        if (distanceToPlayer < enemy.attackDistance && CanAttack())
        {
            stateMachine.ChangeState(enemy.attackState);
            return;
        }
        
        // 使用滞后机制：只有当玩家远离（距离 > 1.5）时才切换回 battle 状态追踪
        // 这样避免了与 battle 状态的 0.5 阈值产生频繁切换
        if (distanceToPlayer > 1.5f)
        {
            stateMachine.ChangeState(enemy.battleState);
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
