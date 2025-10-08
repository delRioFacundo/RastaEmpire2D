using UnityEngine;

public class State_Planting : IState
{
    Player _player;
    public State_Planting(Player player) { this._player = player; }

    public void EnterState()
    {
        _player.PreviusState = _player.CurrentState;
        _player.CurrentState = CurrentState.State_Planting;
        //  _player.playerAnimation.SetAnim(_player.currentState);
        _player.player_Animation.EnableLocomotion(true);
    }

    public void ExitState()
    {
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.ClosePopup, "Interaction");
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

