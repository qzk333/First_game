using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherGroundedState : EnemyState
{
    protected Enemy_Archer enemy;
    protected Transform player;

    public ArcherGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

        float distanceToPlayer = Vector2.Distance(enemy.transform.position, player.position);
        bool isYClose = Mathf.Abs(enemy.transform.position.y - player.position.y) < 2f;

        if ((enemy.IsPlayerDetected() || distanceToPlayer < 10f) && isYClose)
        {
            if (!enemy.IsWallDetected() && enemy.IsGroundDetected())
            {
                stateMachine.ChangeState(enemy.battleState);
                return;
            }
        }
    }
}
