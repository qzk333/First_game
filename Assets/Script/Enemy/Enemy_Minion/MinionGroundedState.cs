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

        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(enemy.transform.position, player.position);
        
        // 增加高度差检测：如果玩家Y轴差距太大（比如大于2），就不进入战斗
        bool isYClose = Mathf.Abs(enemy.transform.position.y - player.position.y) < 2f;

        // 只有当：(看到玩家 或者 距离很近) 并且 (高度差不大) 时才尝试进入战斗
        if ((enemy.IsPlayerDetected() || distanceToPlayer < 10) && isYClose)
        {
            if (!enemy.IsWallDetected() && enemy.IsGroundDetected())
            {
                stateMachine.ChangeState(enemy.battleState);
            }
        }
    }
}