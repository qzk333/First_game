using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss2 : Enemy
{
    [Header("Boss Settings")]
    public Vector2 knockbackForce = new Vector2(5, 2);

    [HideInInspector] public int attackCounter = 0; // 记录攻击次数

    [Header("Charge Settings")]
    public float chargeSpeed = 12f;      // 冲锋速度
    public float chargeDuration = 1.5f;   // 冲锋持续时间
    public float chargeCooldown = 5f;     // 冲锋冷却时间
    [HideInInspector] public float lastTimeCharged; // 记录上次冲锋时间

    #region States
    public Boss2IdleState idleState { get; private set; }
    public Boss2BattleState battleState { get; private set; }
    public Boss2AttackState attackState { get; private set; }
    public Boss2StunnedState stunnedState { get; private set; }
    public Boss2DeadState deadState { get; private set; }
    public Boss2ChargeState chargeState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        // Boss 默认不巡逻，所以不需要 MoveState
        idleState = new Boss2IdleState(this, stateMachine, "Idle", this);
        battleState = new Boss2BattleState(this, stateMachine, "Battle", this);
        attackState = new Boss2AttackState(this, stateMachine, "Attack", this);
        stunnedState = new Boss2StunnedState(this, stateMachine, "Stunned", this);
        deadState = new Boss2DeadState(this, stateMachine, "Die", this);
        chargeState = new Boss2ChargeState(this, stateMachine, "Charge", this);
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
