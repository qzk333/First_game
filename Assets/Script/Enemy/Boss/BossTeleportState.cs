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
        stateTimer = 0.5f; // Fallback: auto-finish if no animation event
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

        // Wait for vanish anim to finish; fallback timer to avoid being stuck if event missing
        if (triggerCalled || stateTimer < 0)
        {
            Transform player = boss.GetPlayerTransform();
            if (player != null)
            {
                // Appear above and slightly in front of the player
                int playerFacing = 1;
                Player playerComp = player.GetComponent<Player>();
                if (playerComp != null)
                    playerFacing = playerComp.facingDir;
                else if (player.localScale.x < 0)
                    playerFacing = -1;

                Vector3 targetPos = new Vector3(
                    player.position.x ,
                    player.position.y + boss.hoverHeight,
                    0);
                boss.transform.position = targetPos;

                // Face toward the player (opposite of player's facing if we appear in front)
                int desiredDir = -playerFacing;
                if (desiredDir == 0)
                    desiredDir = (int)Mathf.Sign(player.position.x - boss.transform.position.x);

                if (desiredDir != 0 && desiredDir != boss.facingDir)
                    boss.Flip();
            }
            stateMachine.ChangeState(boss.smashState);
        }
    }
}
