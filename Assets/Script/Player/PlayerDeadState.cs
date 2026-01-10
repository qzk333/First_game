using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.instance.PlaySFX(6, null);
    }

    public override void Exit()
    {
        base.Exit();

        AudioManager.instance.StopSFX(6);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(0, 0);
    }
}
