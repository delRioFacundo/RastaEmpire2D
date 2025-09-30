using UnityEngine;

public class State_Move : IState
{
  Player _player;
  public State_Move(Player player) { this._player = player; }

  public void EnterState()
  {
    _player.PreviusState = _player.CurrentState;
    _player.CurrentState = CurrentState.State_Move;
    //  _player.playerAnimation.SetAnim(_player.currentState);
    _player.player_Animation.EnableLocomotion(true);
  }

  public void ExitState()
  {
    _player.player_Animation.EnableLocomotion(false);
    // Opcional: dejar mirando hacia donde venía
    _player.player_Animation.ForceIdleFacing(_player.input.Facing);
  }

  public void FixedUpdateState()
  {

  }

  public void LateUpdateState()
  {

  }
  public void UpdateState()
  {
    // Debug.Log("State_Move Update");

    _player.motor.Move(_player.input.Move);

    if (!_player.input.HasMovementInput())
      _player.stateMachine.ChangeState(new State_Idle(_player));

    // ✅ sólo aquí actualizamos anim locomoción
    _player.player_Animation.TickLocomotion(_player.input.Move);



    if (_player.input.ConsumeInteractPressed())
    {
      var target = _player.interactDetector.FindBest(); // <- tu detector (Overlap o Trigger)
      if (target != null)
      {
        _player.currentInteractable = target;         // <- CLAVE
        Debug.Log($"[Move] Target: {_player.currentInteractable}");
        _player.stateMachine.ChangeState(new State_Interact(_player));
      }
      else
      {
        Debug.LogWarning("[Move] Interact presionado pero no hay target.");
      }
    }
    else
    {
      // Igual llamamos al detector para refrescar el cartel
      _player.interactDetector.FindBestAndShowUI();
    }
  }
}

