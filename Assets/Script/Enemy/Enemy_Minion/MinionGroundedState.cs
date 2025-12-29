using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionGroundedState : EnemyState
{
    protected Enemy_Minion enemy;
    protected Transform player;
    public MinionGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Minion _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        if (PlayerManager.instance != null && PlayerManager.instance.player != null)
            player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player == null)
            return;

        // 检测玩家距离
        float distanceToPlayer = Vector2.Distance(enemy.transform.position, player.position);
        
        // grounded 状态用于 move 状态，检测到玩家就切换到 battle
        if (enemy.IsPlayerDetected() || distanceToPlayer < 2)
        {
            stateMachine.ChangeState(enemy.battleState);
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
