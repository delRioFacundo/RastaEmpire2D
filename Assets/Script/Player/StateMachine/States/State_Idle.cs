using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Idle : IState
{
    Player _player;
    public State_Idle(Player player) { this._player = player; }

    public void EnterState()
    {
        _player.PreviusState = _player.CurrentState;
        _player.CurrentState = CurrentState.State_Idle;
        // _player.playerAnimation.SetAnim(_player.CurrentState);
        
    }

    public void ExitState()
    {

    }

    public void FixedUpdateState()
    {

    }

    public void LateUpdateState()
    {

    }
    public void UpdateState()
    {
        // Debug.Log("State_Idle Update");

        if (_player.input.HasMovementInput())
            _player.stateMachine.ChangeState(new State_Move(_player));
        //  _player.stateMachine.ChangeState(new State_Swinging(_player));

        if (_player.input.ConsumeInteractPressed())
        {
            var target = _player.interactDetector.FindBest(); // <- tu detector (Overlap o Trigger)
            if (target != null)
            {
                _player.currentInteractable = target;         // <- CLAVE
                                                              //                Debug.Log($"[Move] Target: {_player.currentInteractable}");
                _player.stateMachine.ChangeState(new State_Interact(_player));
            }
            else
            {
                //  Debug.LogWarning("[Move] Interact presionado pero no hay target.");
            }
        }
        else
        {
            // Igual llamamos al detector para refrescar el cartel
            _player.interactDetector.FindBestAndShowUI();
        }


        if (_player.input.ConsumeSmokePressed())
        {
            _player.stateMachine.ChangeState(new State_Smoke(_player));
        }
    }
}

