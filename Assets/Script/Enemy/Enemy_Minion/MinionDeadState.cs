using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionDeadState : EnemyState
{
    private Enemy_Minion enemy;

    public MinionDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Minion _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    int playOnce = 1;

    public override void Enter()
    {
        base.Enter();

        // 移除了以下错误代码，让死亡动画正常播放：
        // enemy.anim.SetBool(enemy.lastAnimBoolName, true);  // 这会设置错误的动画
        // enemy.anim.speed = 0;  // 这会让所有动画停止播放
        //enemy.cd.enabled = false;  // 注释掉，保留死亡后的碰撞体

        stateTimer = .1f;  // ������ʬ����䣬ֵԽС������Խ��
        
        //rb.bodyType = RigidbodyType2D.Kinematic;  // 禁用物理模拟，让敌人停留在原地

        enemy.SetVelocity(0,rb.velocity.y); // 停止所有移动

        if(playOnce -- >0)
            AudioManager.instance.PlaySFX(9, null);
    }

    public override void Update()
    {
        base.Update();

        // 注释掉向上飞的效果，让敌人自然掉落
        // if (stateTimer > 0)
        //     rb.velocity = new Vector2(0, 3);
    }
}
