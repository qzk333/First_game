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

        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position ) < 2)
            stateMachine.ChangeState(enemy.battleState);
    }
}
