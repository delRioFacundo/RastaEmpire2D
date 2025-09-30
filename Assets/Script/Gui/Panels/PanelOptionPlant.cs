using UnityEngine;

public class PanelOptionPlant : MonoBehaviour
{
    public bool planting, watering, fertilizing, harvesting, protectionSpray;
    void Start()
    {

    }

    void Update()
    {

    }

    public void Planting()
    {
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.CambiarPanel, "Panel Planting");
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.ChangeStatePlayer, "Planting"); // volver al estado Idle del jugador

    }
    public void Watering()
    {
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.CambiarPanel, "State_Water");
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.ChangeStatePlayer, "Water"); // volver al estado Idle del jugador
    }
    public void Fertilizing()
    {
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.CambiarPanel, "Panel Fertilizing");
    }
    public void Harvesting()
    {
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.CambiarPanel, "Panel Harvest");
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.ChangeStatePlayer, "Harvest"); // volver al estado Idle del jugador

    }
    public void ProtectionSpray()
    {
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.CambiarPanel, "Panel Protection Spray");
    }
    public void Back()
    {
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.CambiarPanel, "Panel Game"); // volver al panel del juego    
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.ChangeStatePlayer, "Idle"); // volver al estado Idle del jugador

    }
}
