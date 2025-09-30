using UnityEngine;

public class State_Water : IState
{
    Player _player;
    public State_Water(Player player) { this._player = player; }

    public void EnterState()
    {
        _player.PreviusState = _player.CurrentState;
        _player.CurrentState = CurrentState.State_Water;
        //  _player.playerAnimation.SetAnim(_player.currentState);

        // Forzar idle con facing actual (para que no se mueva mientras riega)
        _player.player_Animation.ForceIdleFacing(_player.input.Facing);
        // Activar animación de regar
        _player.player_Animation.SetWatering(true);

        // 2) Elegir la posición según dir/left-right y mover al jugador
        var plant = _player.CurrentPlant;
        if (plant != null)
        {
            Transform target = null;
            int dir = _player.player_Animation.GetCurrentDir(); // 0=Down,1=Up,2=Side

            switch (dir)
            {
                case 0: target = plant.PosDown; break; // mirando hacia abajo → pararse abajo
                case 1: target = plant.PosUp; break; // mirando hacia arriba → pararse arriba
                case 2: // lateral: elegir Left/Right según flip/facing
                    target = _player.player_Animation.IsFacingLeftOnSide()
                           ? plant.PosLeft
                           : plant.PosRight;
                    break;
            }

            if (target != null) _player.transform.position = target.position;
        }


    }

    public void ExitState()
    {
        _player.player_Animation.SetWatering(false);
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

