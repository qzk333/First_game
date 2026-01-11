using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2DeadState : EnemyState
{
    private Enemy_Boss2 enemy;
    private int playOnce = 1;

    public Boss2DeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Boss2 _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = .1f;
        enemy.SetVelocity(0, rb.velocity.y);

        if (playOnce-- > 0 && AudioManager.instance != null)
            AudioManager.instance.PlaySFX(7, null);
    }
}
