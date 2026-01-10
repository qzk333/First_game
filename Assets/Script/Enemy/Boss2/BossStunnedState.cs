using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStunnedState : EnemyState
{
    private Enemy_Boss enemy;

    public BossStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Boss _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        // 视觉效果：调用 EntityFX 让 Boss 闪烁红光
        enemy.fx.InvokeRepeating("RedColorBlink", 0, .1f);

        stateTimer = enemy.stunDuration;

        // 物理效果：受击弹开
        rb.velocity = new Vector2(-enemy.facingDir * enemy.stunDirection.x, enemy.stunDirection.y);
    }

    public override void Update()
    {
        base.Update();

        // 眩晕结束后回到静止状态（等待重新发现玩家）
        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.idleState);
    }

    public override void Exit()
    {
        base.Exit();
        // 停止闪烁红光
        enemy.fx.Invoke("CancelColorChange", 0);
    }
}
