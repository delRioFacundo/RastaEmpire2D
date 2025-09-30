using UnityEngine;

public class PanelLogin : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    public void OnClickLogin()
    {
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.Login);
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.LoginSuccess);
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.WalletId, "0x1234567890abcdef");
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.CambiarPanel, "Panel Cargando");
    }
}
