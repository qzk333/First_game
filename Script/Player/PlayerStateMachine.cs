using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentstate { get; private set; }

    public void Initialize(PlayerState _startState)
    {
        currentstate = _startState;
        currentstate.Enter();
    }
    
    public void ChangeState(PlayerState _newState)
    {
        currentstate.Exit();
        currentstate = _newState;
        currentstate.Enter();
    }
}
