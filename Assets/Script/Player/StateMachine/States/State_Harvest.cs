using UnityEngine;

public class State_Harvest : IState
{
    Player _player;
    public State_Harvest(Player player) { this._player = player; }
    PlantHarvestUI _plant;
    public void EnterState()
    {
        _player.PreviusState = _player.CurrentState;
        _player.CurrentState = CurrentState.State_Harvest;
        // _player.playerAnimation.SetAnim(_player.CurrentState);
        _player.motor.StopInstant();
        // La planta objetivo viene seteada antes de entrar al estado:
        _plant = _player.plantHarvest;
        _player.cursorManager.SetHarvestCursor();

    }

    public void ExitState()
    {
        _plant = null;
        _player.cursorManager.SetDefaultCursor();
    }

    public void FixedUpdateState()
    {

    }

    public void LateUpdateState()
    {

    }
    public void UpdateState()
    {


        // Terminado -> volver a Idle
        if (_plant.AllHarvested)
            _player.stateMachine.ChangeState(new State_Idle(_player));

        if (_player.input.ConsumeClickPressed())
        {
            _player.cursorManager.SetCursorAnim();
        }
    }
}

