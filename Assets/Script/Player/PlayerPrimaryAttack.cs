using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttack : PlayerState
{
    private int comboCounter;
    private float lastTimeAttacked;
    private float comboWindow = 2;

    public PlayerPrimaryAttack(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        
        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;
            
        player.anim.SetInteger("ComboCounter", comboCounter);
        
        #region Choose attack direction
        float attackDir = player.facingDir;
        if (xInput != 0)
            attackDir = xInput;
        
        // 强制同步角色朝向，确保攻击方向和视觉一致
        if (attackDir > 0 && player.facingDir < 0)
            player.Flip();
        else if (attackDir < 0 && player.facingDir > 0)
            player.Flip();
        #endregion
        
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, 
                          player.attackMovement[comboCounter].y);
        stateTimer = .1f;
    }
    
    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .15f);
        player.anim.speed = 1;
        comboCounter++;
        lastTimeAttacked = Time.time;
    }
    
    public override void Update()
    {
        base.Update();
        
        if (stateTimer < 0)
            player.SetVelocity(0, 0);
        
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}