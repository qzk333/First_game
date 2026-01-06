using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = .2f;
    
    [Header("Ranged Attack")]
    public GameObject swordWavePrefab; // 刀波预制体
    public float rangedDamageMultiplier = 3.0f; // 3倍伤害
    public float defaultGravityScale; // 记录默认重力

    [Header("Heal info")]
    public float healDuration = 1.0f; // 蓄力时间 1秒
    [Range(0,1)]
    public float healPercent = 0.25f; // 每次回血 10%

    public bool isBusy { get; private set; } 
    [Header("Move info")]
    public float moveSpeed = 12f;
    public float jumpForce;

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
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        // base.SlowEntityBy(_slowPercentage, _slowDuration);
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed",_slowDuration);
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

    private void CheckForDashInput()
    {
        if (IsWallDetected())
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
    }

}
