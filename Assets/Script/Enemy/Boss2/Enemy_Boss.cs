using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss : Enemy
{
    [Header("Boss Settings")]
    public Vector2 knockbackForce = new Vector2(5, 2);

    [HideInInspector] public int attackCounter = 0; // 记录攻击次数

    #region States
    public BossIdleState idleState { get; private set; }
    public BossBattleState battleState { get; private set; }
    public BossAttackState attackState { get; private set; }
    public BossStunnedState stunnedState { get; private set; }
    public BossDeadState deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        // Boss 默认不巡逻，所以不需要 MoveState
        idleState = new BossIdleState(this, stateMachine, "Idle", this);
        battleState = new BossBattleState(this, stateMachine, "Battle", this);
        attackState = new BossAttackState(this, stateMachine, "Attack", this);
        stunnedState = new BossStunnedState(this, stateMachine, "Stunned", this);
        deadState = new BossDeadState(this, stateMachine, "Die", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }
}
