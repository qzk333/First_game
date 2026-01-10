using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2DeadState : EnemyState
{
    private Enemy_Boss2 enemy;

    public Boss2DeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Boss2 _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    int playOnce = 1;
    public override void Enter()
    {
        base.Enter();

        stateTimer = .1f;

        enemy.SetVelocity(0, rb.velocity.y); // 停止所有移动

        if (playOnce-- > 0)
            AudioManager.instance.PlaySFX(9, null);

        // 如果你的 Boss 死亡后要禁用碰撞体，可以取消下面注释
        // enemy.cd.enabled = false;
    }

    public override void Update()
    {
        base.Update();

        // 确保 Boss 尸体不再受玩家推动
        //enemy.SetVelocity(0, rb.velocity.y);
    }
}
