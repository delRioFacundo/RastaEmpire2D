using UnityEngine;

public class State_Interact : IState

{
    Player _player;
    public State_Interact(Player player) { this._player = player; }

    public void EnterState()
    {
        _player.PreviusState = _player.CurrentState;
        _player.CurrentState = CurrentState.State_Interact;
        //  _player.playerAnimation.SetAnim(_player.currentState);
        // Cortar movimiento y reproducir animación si querés
        _player.motor.StopInstant();
       /*  if (_player.currentInteractable == null)
        {
            Debug.LogWarning("[Interact] currentInteractable == null");
            _player.stateMachine.ChangeState(new State_Idle(_player));
            return;
        } */

     //   Debug.Log($"[Interact] Llamando Interact() en: {_player.currentInteractable}");
        _player.currentInteractable.Interact(_player);
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

      //  Debug.Log("State_Interact Update");
        // Si no hay interacción en curso, volver a Idle


    }
}
