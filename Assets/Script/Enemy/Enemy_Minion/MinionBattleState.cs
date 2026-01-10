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
        stateTimer = enemy.battleTime; 
        if (PlayerManager.instance != null && PlayerManager.instance.player != null)
            player = PlayerManager.instance.player.transform;
    }
    public override void Update()
    {
        base.Update();

        if (player == null) return;

        // 1. 视距与脱战逻辑
        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack()) stateMachine.ChangeState(enemy.attackState);
                return; // 切换状态后必须 return
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 10)
            {
                stateMachine.ChangeState(enemy.idleState);
                return; // return
            }
        }

        // 2. 计算移动方向
        if (player.position.x > enemy.transform.position.x) moveDir = 1;
        else if (player.position.x < enemy.transform.position.x) moveDir = -1;

        // 3. 悬崖/墙壁检测 (防掉落核心)
        // 注意：这里我们检测的是"前方是否有路"，如果没路，直接切 Idle 并停止
        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            // ！！！重要修改！！！
            // 不要在这里 Flip()，否则会背对玩家，导致 GroundedState 认为背后有路从而再次进入 Battle 造成死循环
            // 也不要继续移动
            enemy.SetVelocity(0, rb.velocity.y); // 立即刹车
            stateMachine.ChangeState(enemy.idleState);
            return; // ！！！关键：必须终止代码执行，防止下面的 SetVelocity 继续生效
        }

        // 4. 转身逻辑 (只有确认前方安全才转身追击)
        if (moveDir != enemy.facingDir)
            enemy.Flip();

        // 5. 距离太近切 Idle (防止重叠)
        if (Mathf.Abs(player.position.x - enemy.transform.position.x) < 0.5f)
        {
            stateMachine.ChangeState(enemy.idleState);
            return;
        }

        // 6. 执行移动
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
