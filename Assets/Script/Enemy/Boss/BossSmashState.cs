using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSmashState : BossState
{
    public BossSmashState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Boss _boss) : base(_enemyBase, _stateMachine, _animBoolName, _boss)
    {
    }

    public override void Enter()
    {
        base.Enter();
        boss.rb.gravityScale = 5; // Fall faster
        AudioManager.instance.PlaySFX(17, null);
    }

    public override void Exit()
    {
        base.Exit();
        boss.rb.gravityScale = 1; // Reset gravity
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            DoRetreatStep();
            stateMachine.ChangeState(boss.battleState);
        }
    }

    private void DoRetreatStep()
    {
        Transform player = boss.GetPlayerTransform();
        float retreatDir = -boss.facingDir; // default: step backward from current facing

        if (player != null)
        {
            float dirFromPlayer = Mathf.Sign(boss.transform.position.x - player.position.x);
            if (dirFromPlayer != 0)
                retreatDir = dirFromPlayer; // move away from player
        }

        boss.transform.position += new Vector3(retreatDir * boss.retreatDistance, 0, 0);
    }
}
