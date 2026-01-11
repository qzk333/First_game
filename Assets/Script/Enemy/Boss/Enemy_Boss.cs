using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss : Enemy
{
    #region States
    public BossIdleState idleState { get; private set; }
    public BossMoveState moveState { get; private set; }
    public BossBattleState battleState { get; private set; }
    public BossAttackState attackState { get; private set; }
    public BossDashState dashState { get; private set; }
    public BossTeleportState teleportState { get; private set; }
    public BossSmashState smashState { get; private set; }
    public BossDeadState deadState { get; private set; }
    #endregion

    [Header("Skill Info")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.5f;
    public float dashCooldown = 5f;
    [HideInInspector] public float lastDashTime;

    [Header("Teleport Smash Info")]
    public float smashDamageRadius = 3f;
    public int smashDamage = 20;
    public float teleportCooldown = 10f;
    public float hoverHeight = 3f; // Height above player to reappear
    public float teleportOffset = 1.5f; // Horizontal offset in front of player when reappearing
    public float retreatDistance = 2f; // Step back after smash
    [HideInInspector] public float lastTeleportTime;

    protected override void Awake()
    {
        base.Awake();

        idleState = new BossIdleState(this, stateMachine, "Idle", this);
        moveState = new BossMoveState(this, stateMachine, "Move", this);
        battleState = new BossBattleState(this, stateMachine, "Move", this); // Battle often uses Move/Idle anims but logic differs
        attackState = new BossAttackState(this, stateMachine, "Attack", this);
        dashState = new BossDashState(this, stateMachine, "Dash", this);
        teleportState = new BossTeleportState(this, stateMachine, "Teleport", this);
        smashState = new BossSmashState(this, stateMachine, "Smash", this);
        deadState = new BossDeadState(this, stateMachine, "Die", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        lastDashTime = -dashCooldown; // Ready immediately
        lastTeleportTime = -teleportCooldown; // Ready immediately
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    // Skill Checks
    public bool CanDash()
    {
        if (Time.time > lastDashTime + dashCooldown)
        {
            return true;
        }
        return false;
    }

    public bool CanTeleport()
    {
        if (Time.time > lastTeleportTime + teleportCooldown)
        {
            return true;
        }
        return false;
    }

    // Helper to find player position directly for teleport
    public Transform GetPlayerTransform()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            return player.transform;
        return null;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
