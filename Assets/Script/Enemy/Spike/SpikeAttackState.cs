using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeAttackState : EnemyState
{
    private SpikeTrap enemy;

    public SpikeAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, SpikeTrap _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.cooldownDuration;

        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius, enemy.PlayerLayer);

        foreach (var hit in hitPlayers)
        {
            CharacterStats targetStats = hit.GetComponent<CharacterStats>();

            if (targetStats != null)
            {
                // 不再计算伤害数值，直接调用处决方法
                targetStats.KillImmediately();

                EntityFX fx = hit.GetComponentInChildren<EntityFX>();
                if (fx != null)
                    fx.StartCoroutine("FlashFX");
            }
        }
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
