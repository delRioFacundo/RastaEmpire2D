using System.Collections.Generic;

public class EventsManagerSpaar
{
    public delegate void EventReceiver(params object[] parameterContainer);
    private static Dictionary<EventTypeSpaar, EventReceiver> _events;

    public static void SubscribeToEvent(EventTypeSpaar eventType, EventReceiver listener)
    {
        if (_events == null)
            _events = new Dictionary<EventTypeSpaar, EventReceiver>();

        if (!_events.ContainsKey(eventType))
            _events.Add(eventType, null);

        _events[eventType] += listener;
    }

    public static void UnsubscribeToEvent(EventTypeSpaar eventType, EventReceiver listener)
    {
        if (_events != null)
        {
            if (_events.ContainsKey(eventType))
                _events[eventType] -= listener;
        }
    }

    public static void TriggerEvent(EventTypeSpaar eventType)
    {
        TriggerEvent(eventType, null);
    }

    public static void TriggerEvent(EventTypeSpaar eventType, params object[] parametersWrapper)
    {
        if (_events == null)
        {
            UnityEngine.Debug.LogWarning("No events subscribed");
            return;
        }

        if (_events.ContainsKey(eventType))
        {
            if (_events[eventType] != null)
                _events[eventType](parametersWrapper);
        }
    }
}

public enum EventTypeSpaar
{
    CambiarPanel,
    PanelAnterior,
    panelActual,
    UnsubscribeToEvent,
    EstadoDeJuego,

    /////////// LOGIN ///////
    Login,
    LoginSuccess,
    WalletId,


    //// ACCIONES DEL JUEGO /////////

    Sembrar,

    Cosechar,

    HarvestComplete,

    UpdatePanelOptionsPlant,



    ShaderSmoked,

    /// Data Base /////

    GetDataBase,

    SetDataBase,

    GetDataBaseSuccess,


    /////// PLAYER ////////

    ChangeStatePlayer,


    ///////// UI //////////

    OpenPopup,
    ClosePopup,


    ///////Plant ////////
    
    PlantDetected, 

}