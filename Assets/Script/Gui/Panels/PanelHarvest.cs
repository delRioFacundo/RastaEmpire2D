using UnityEngine;

public class PanelHarvest : MonoBehaviour
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
