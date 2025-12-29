using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionBattleState : EnemyState
{
    private Transform player;
    private Enemy_Minion enemy;
    private int moveDir;
    public MinionBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Minion _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        if (PlayerManager.instance != null && PlayerManager.instance.player != null)
            player = PlayerManager.instance.player.transform;
    }
    public override void Update()
    {
        base.Update();

        if (player == null)
            return;

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;

            if(enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if(CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position,enemy.transform.position) > 10)
                stateMachine.ChangeState(enemy.idleState);
        }

        // 根据玩家位置决定移动方向和转身
        float xDistanceToPlayer = player.position.x - enemy.transform.position.x;
        float absDistance = Mathf.Abs(xDistanceToPlayer);
        
        // 如果距离太近，切换到idle状态
        if (absDistance < 0.5f)
        {
            stateMachine.ChangeState(enemy.idleState);
            return;
        }
        
        // 根据玩家位置决定朝向
        if (xDistanceToPlayer > 0.1f)  // 玩家在右边
            moveDir = 1;
        else if(xDistanceToPlayer < -0.1f)  // 玩家在左边
            moveDir = -1;

        // 确保敌人面向玩家
        if (moveDir != enemy.facingDir)
            enemy.Flip();

        // 正常移动追踪玩家
        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
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
