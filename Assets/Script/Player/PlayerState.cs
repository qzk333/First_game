using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    protected Rigidbody2D rb;

    public float xInput;
    public float yInput;
    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine,string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        // 使用新的 Input System 获取移动输入
        if (InputManager.instance != null && InputManager.instance.playerActions != null)
        {
            Vector2 movement = InputManager.instance.playerActions.Player.Movement.ReadValue<Vector2>();
            xInput = movement.x;
            yInput = movement.y;
        }
        player.anim.SetFloat("yVelocity",rb.velocity.y);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);

    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
