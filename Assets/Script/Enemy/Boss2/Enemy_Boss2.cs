using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss2 : Enemy
{
    [Header("Dash")]
    public float dashSpeed = 8f;
    public float dashDuration = 0.8f;
    public float dashCooldownTime = 3f;
    public int dashDamage = 20;
    public float dashHitRadius = 0.75f;
    [SerializeField] private Transform dashHitCheck;
    [SerializeField] private Transform playerCheckPoint;

    private float lastDashTime = -Mathf.Infinity;
    private bool dashDamageAppliedThisDash;

    #region States
    public Boss2IdleState idleState { get; private set; }
    public Boss2MoveState moveState { get; private set; }
    public Boss2BattleState battleState { get; private set; }
    public Boss2AttackState attackState { get; private set; }
    public Boss2DashState dashState { get; private set; }
    public Boss2StunnedState stunnedState { get; private set; }
    public Boss2DeadState deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new Boss2IdleState(this, stateMachine, "Idle", this);
        moveState = new Boss2MoveState(this, stateMachine, "Move", this);
        battleState = new Boss2BattleState(this, stateMachine, "Battle", this);
        attackState = new Boss2AttackState(this, stateMachine, "Attack", this);
        dashState = new Boss2DashState(this, stateMachine, "Dash", this);
        stunnedState = new Boss2StunnedState(this, stateMachine, "Stunned", this);
        deadState = new Boss2DeadState(this, stateMachine, "Die", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
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

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    public bool CanDash() => Time.time >= lastDashTime + dashCooldownTime;

    public void RecordDash() => lastDashTime = Time.time;

    public Transform PlayerCheckOrigin => playerCheckPoint != null ? playerCheckPoint : transform;

    public Transform DashHitPoint => dashHitCheck != null ? dashHitCheck : attackCheck;

    public LayerMask PlayerLayer => whatIsPlayer;

    public void ResetDashDamageFlag() => dashDamageAppliedThisDash = false;

    public bool TryDealDashDamage()
    {
        if (dashDamageAppliedThisDash)
            return false;

        Collider2D[] hits = Physics2D.OverlapCircleAll(DashHitPoint.position, dashHitRadius, PlayerLayer);
        foreach (var hit in hits)
        {
            PlayerStats target = hit.GetComponent<PlayerStats>();
            if (target != null)
            {
                target.TakeDamage(dashDamage);
                dashDamageAppliedThisDash = true;
                return true;
            }
        }

        return false;
    }

    public override RaycastHit2D IsPlayerDetected()
    {
        Vector2 origin = PlayerCheckOrigin.position;
        return Physics2D.Raycast(origin, Vector2.right * facingDir, playerCheckDistance, whatIsPlayer);
    }

    protected override void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        }

        if (wallCheck != null)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
        }

        if (attackCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
        }

        if (DashHitPoint != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(DashHitPoint.position, dashHitRadius);
        }

        Transform playerOrigin = PlayerCheckOrigin;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(playerOrigin.position,
            new Vector3(playerOrigin.position.x + playerCheckDistance * facingDir, playerOrigin.position.y));
    }
}
