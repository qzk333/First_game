using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : Enemy
{
    [Header("Spike Settings")]
    public int damage = 10;
    public float cooldownDuration = 2f;

    public LayerMask PlayerLayer => whatIsPlayer;

    #region States
    public SpikeIdleState idleState { get; private set; }
    public SpikeAttackState attackState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        // 传递空字符串，因为没有动画
        idleState = new SpikeIdleState(this, stateMachine, "", this);
        attackState = new SpikeAttackState(this, stateMachine, "", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }


    // 核心：由 Trigger Collider 触发
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 只有在待机状态下检测到玩家层级才触发攻击
        Debug.Log("111");
        if (stateMachine.currentState == idleState && (1 << other.gameObject.layer & whatIsPlayer) != 0)
        {
            stateMachine.ChangeState(attackState);
        }
    }
}
