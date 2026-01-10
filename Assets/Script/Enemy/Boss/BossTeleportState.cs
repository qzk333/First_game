using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTeleportState : BossState
{
    public BossTeleportState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Boss _boss) : base(_enemyBase, _stateMachine, _animBoolName, _boss)
    {
    }

    public override void Enter()
    {
        base.Enter();
        boss.stats.MakeInvincible(true);
        // Usually play a "vanish" anim here
    }

    public override void Exit()
    {
        base.Exit();
        boss.stats.MakeInvincible(false);
        boss.lastTeleportTime = Time.time;
    }

    public override void Update()
    {
        base.Update();
        boss.SetVelocity(0, 0);

        if (triggerCalled) // Wait for vanish anim to finish
        {
            Transform player = boss.GetPlayerTransform();
            if (player != null)
            {
                boss.transform.position = new Vector3(player.position.x, player.position.y + boss.hoverHeight, 0);
            }
            stateMachine.ChangeState(boss.smashState);
        }
    }
}
