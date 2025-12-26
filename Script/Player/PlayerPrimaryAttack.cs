using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerPrimaryAttack : PlayerState
{
    private int comboCounter;
    private float lastTimeAttacked;
    private float comboWindow = 2;
    
    // 用于缓存连击输入
    private bool comboRequested = false;
    
    public PlayerPrimaryAttack(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        xInput = 0;
        comboRequested = false; // 重置连击请求
        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;
            
        Debug.Log($"<color=cyan>[Attack Enter]</color> comboCounter = {comboCounter}");
        player.anim.SetInteger("ComboCounter", comboCounter);
        
        #region Choose attack direction
        float attackDir = player.facingDir;
        if (xInput != 0)
            attackDir = xInput;
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
        Debug.Log($"<color=yellow>[Attack Exit]</color> comboCounter increased to {comboCounter}");
    }
    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            player.SetZeroVelocity();
        // 检测连击输入 - 在攻击动画播放中途就可以缓存下一次攻击
        if (Input.GetKeyDown(KeyCode.J))
        {
            comboRequested = true;
            Debug.Log($"<color=green>[Attack]</color> Combo requested!");
        }
        if (triggerCalled)
        {
            Debug.Log($"<color=magenta>[Attack]</color> Trigger called! comboRequested={comboRequested}, comboCounter={comboCounter}");
            // 如果缓存了连击请求，继续下一段攻击
            if (comboRequested && comboCounter < 2) // 最多3段攻击（0, 1, 2）
            {
                Debug.Log($"<color=orange>[Attack]</color> Switching to next combo!");
                stateMachine.ChangeState(player.primaryAttack);
            }
            else
            {
                Debug.Log($"<color=red>[Attack]</color> Going to idle");
                stateMachine.ChangeState(player.idleState);
            }
        }
    }
}