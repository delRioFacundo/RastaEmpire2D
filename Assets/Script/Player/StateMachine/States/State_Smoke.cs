using UnityEngine;

public class State_Smoke : IState
{
    Player _player;
    public State_Smoke(Player player) { this._player = player; }

    public void EnterState()
    {
        _player.PreviusState = _player.CurrentState;
        _player.CurrentState = CurrentState.State_Smoke;
        //  _player.playerAnimation.SetAnim(_player.currentState);
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.ShaderSmoked, true);
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

    }
}

