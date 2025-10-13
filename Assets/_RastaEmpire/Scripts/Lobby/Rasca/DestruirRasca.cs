using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestruirRasca : MonoBehaviour
{
    public GameObject rasca;
    public GameObject prefab;
    public DataPlayer dataPlayer;
    // Start is called before the first frame update
    void Awake()
    {
       // dataPlayer = GameObject.Find("Data Player").GetComponent<DataPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        dataPlayer = GameObject.Find("Data Player").GetComponent<DataPlayer>();
    }

    public void Destruir()
    {
       /*  if (!dataPlayer.startModeGestion)
        {
            EventsManager.TriggerEvent(EventType.Panels, "c");
           // EventsManager.TriggerEvent(EventType.Panels, "");
            EventsManager.TriggerEvent(EventType.OpenInventory, new object[] { 0, false, true, false });
        }
        else
        {
            // EventsManager.TriggerEvent(EventType.Panels, "Panel Plantation_Pc");
            EventsManager.TriggerEvent(EventType.OpenInventory, new object[] { 0, false, true, false });
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        EventsManager.TriggerEvent(EventType.BlockPlayerInput, false);

        Destroy(rasca); */
    }

    public void Crear()
    {
        GameObject rascas = Instantiate(prefab, Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").GetComponent<Canvas>().transform);
        rascas.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        //rascas.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
    }
}
