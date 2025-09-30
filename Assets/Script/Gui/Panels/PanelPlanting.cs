using UnityEngine;

public class PanelPlanting : MonoBehaviour
{
   [SerializeField] string panelName;

    void Start()
    {

    }

    void Update()
    {

    }

    public void OnClickClose()
    {
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.ClosePopup, panelName);
    }

    
}
