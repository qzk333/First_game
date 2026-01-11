using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Archer : Enemy
{
    [Header("Ranged Attack")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float arrowSpeed = 8f;
    [SerializeField] private float arrowLifeTime = 4f;
    [SerializeField] private Transform arrowSpawnPoint;

    #region States
    public ArcherIdleState idleState { get; private set; }
    public ArcherMoveState moveState { get; private set; }
    public ArcherBattleState battleState { get; private set; }
    public ArcherAttackState attackState { get; private set; }
    public ArcherStunnedState stunnedState { get; private set; }
    public ArcherDeadState deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new ArcherIdleState(this, stateMachine, "Idle", this);
        moveState = new ArcherMoveState(this, stateMachine, "Move", this);
        battleState = new ArcherBattleState(this, stateMachine, "Idle", this);
        attackState = new ArcherAttackState(this, stateMachine, "Attack", this);
        stunnedState = new ArcherStunnedState(this, stateMachine, "Stunned", this);
        deadState = new ArcherDeadState(this, stateMachine, "Die", this);
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

    public void FireArrow()
    {
        Transform spawnPoint = arrowSpawnPoint != null ? arrowSpawnPoint : attackCheck;
        if (arrowPrefab == null || spawnPoint == null)
            return;

        GameObject arrowObj = Instantiate(arrowPrefab, spawnPoint.position, Quaternion.identity);
        Vector2 direction = new Vector2(facingDir, 0f).normalized;

        ArcherArrow projectile = arrowObj.GetComponent<ArcherArrow>();
        if (projectile != null)
        {
            projectile.Setup(direction, arrowSpeed, arrowLifeTime, stats, whatIsPlayer);
        }
        else
        {
            Rigidbody2D arrowRb = arrowObj.GetComponent<Rigidbody2D>();
            if (arrowRb != null)
                arrowRb.velocity = direction * arrowSpeed;
        }

        Vector3 scale = arrowObj.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * facingDir;
        arrowObj.transform.localScale = scale;
    }

    protected override void OnDrawGizmos()
    {
        // Keep ground and wall debug lines consistent with base but replace attackCheck with a line
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(groundCheck.position,
                new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        }

        if (wallCheck != null)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawLine(wallCheck.position,
                new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
        }

        if (attackCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(attackCheck.position,
                attackCheck.position + Vector3.right * facingDir * attackCheckRadius);
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position,
            new Vector3(transform.position.x + playerCheckDistance * facingDir, transform.position.y));
    }
}
