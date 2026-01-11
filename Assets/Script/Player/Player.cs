using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : Entity
{
    public static event Action OnPlayerDied;
    [Header("Attack details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = .2f;

    [Header("Ranged Attack")]
    public GameObject swordWavePrefab; // 刀波预制体
    public float rangedDamageMultiplier = 3.0f; // 3倍伤害
    public float defaultGravityScale; // 记录默认重力

    [Header("Heal info")]
    public float healDuration = 1.0f; // 蓄力时间 1秒
    [Range(0, 1)]
    public float healPercent = 0.25f; // 每次回血 10%

    public bool isBusy { get; private set; }
    [Header("Move info")]
    public float moveSpeed = 12f;
    public float jumpForce;

    // 隐藏父类的 stats，使用具体的 PlayerStats 类型
    public PlayerStats stats;

    public float defaultMoveSpeed;
    public float defaultJumpForce;

    [Header("Double Jump info")]    // 二段跳相关设置
    public bool canDoubleJump = false;    // 是否可以进行二段跳
    public bool hasDoubleJumped = false;    // 是否已经进行了二段跳

    [Header("Dash info")]
    [SerializeField] private float dashCooldown;
    private float dashUsageTimer;
    public float dashSpeed;
    public float dashDuration;
    public float defaultDashSpeed;
    public float dashDir { get; private set; }

    [Header("Safe Position Info")]
    public Vector3 safePosition;
    private float safePosCheckTimer;

    [Header("Fall Info")]
    public float fallThreshold = -10.0f;
    public int fallDamage = 10;



    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerFallState fallState { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerPrimaryAttack primaryAttack { get; private set; }
    public PlayerJumpAttackState jumpAttackState { get; private set; }
    public PlayerRangedAttackState rangedAttackState { get; private set; }
    public PlayerCounterAttack counterAttack { get; private set; }
    public PlayerHealState healState { get; private set; } // 新增回血状态




    public PlayerDeadState deadState { get; private set; }


    #endregion
    protected override void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();

        // 获取 PlayerStats 组件赋值给新的 stats 变量
        // 这样 Player 脚本中访问 stats 时直接得到 PlayerStats 类型
        stats = GetComponent<PlayerStats>();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        fallState = new PlayerFallState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");

        primaryAttack = new PlayerPrimaryAttack(this, stateMachine, "Attack");
        counterAttack = new PlayerCounterAttack(this, stateMachine, "CounterAttack");
        jumpAttackState = new PlayerJumpAttackState(this, stateMachine, "JumpAttack");
        rangedAttackState = new PlayerRangedAttackState(this, stateMachine, "RangedAttack");
        healState = new PlayerHealState(this, stateMachine, "Heal"); // 初始化回血状态

        deadState = new PlayerDeadState(this, stateMachine, "Die");

    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
        defaultGravityScale = rb.gravityScale; // 初始化重力
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentstate.Update();

        CheckForDashInput();

        UpdateSafePosition();
        CheckForFall();
    }

    private void CheckForFall()
    {
        if (transform.position.y < fallThreshold && !isBusy)
        {
            stats.TakeDamage(fallDamage);
            RespawnAtSafePosition();
        }
    }

    private void UpdateSafePosition()
    {
        safePosCheckTimer -= Time.deltaTime;
        if (safePosCheckTimer < 0)
        {
            safePosCheckTimer = 1f; // Check every 1 second

            if (IsGroundDetected())
            {
                safePosition = transform.position;
            }
        }
    }

    public void RespawnAtSafePosition()
    {
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        isBusy = true; // 禁用输入
        rb.velocity = Vector2.zero; // 停止移动
        rb.gravityScale = 0; // 停止受重力影响

        // 播放受击闪烁 (Entity 中已有 FlashFX，或者直接在此处调用)
        if (fx != null)
            fx.StartCoroutine("FlashFX");

        yield return new WaitForSeconds(1.0f); // 停顿1秒

        transform.position = safePosition; // 传送

        // 恢复
        rb.gravityScale = defaultGravityScale;
        isBusy = false;

        stateMachine.ChangeState(idleState);
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        // base.SlowEntityBy(_slowPercentage, _slowDuration);
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currentstate.AnimationFinishTrigger();

    [Header("Skill Unlock Info")]
    public bool unlockDash = false; // 默认锁定冲刺
    public bool unlockRangedAttack = false; // 默认锁定远程攻击

    private void CheckForDashInput()
    {
        if (IsWallDetected())
            return;

        // 检查冲刺是否解锁
        if (!unlockDash)
            return;

        dashUsageTimer -= Time.deltaTime;

        if (InputManager.instance != null &&
            InputManager.instance.playerActions.Player.Dash.WasPressedThisFrame() &&
            dashUsageTimer < 0)
        {
            dashUsageTimer = dashCooldown;

            Vector2 movement = InputManager.instance.playerActions.Player.Movement.ReadValue<Vector2>();
            dashDir = movement.x;

            if (dashDir == 0)
                dashDir = facingDir;

            stateMachine.ChangeState(dashState);
        }
    }

    #region doubleJump
    // 重置二段跳
    public void ResetDoubleJump()
    {
        canDoubleJump = true;
        hasDoubleJumped = false;
    }

    // 执行二段跳
    public void PerformDoubleJump()
    {
        if (canDoubleJump && !hasDoubleJumped)
        {
            hasDoubleJumped = true;
            canDoubleJump = false; // 二段跳只能使用一次
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // 直接设置速度，不切换状态
        }
    }
    #endregion

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
        OnPlayerDied?.Invoke();
    }

}
