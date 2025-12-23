using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Minion : Enemy
{
    #region States

    public MinionIdleState idleState { get; private set; }
    public MinionMoveState moveState { get; private set; }
    public MinionBattleState battleState { get; private set; }
    public MinionAttackState attackState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new MinionIdleState(this, stateMachine, "Idle", this);
        moveState = new MinionMoveState(this, stateMachine, "Move", this);
        battleState = new MinionBattleState(this, stateMachine, "Battle", this);
        attackState = new MinionAttackState(this, stateMachine, "Attack", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }
}
