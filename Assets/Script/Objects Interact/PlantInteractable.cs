using UnityEngine;

public class PlantInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] Transform anchor, posLeft, posRight, posUp, posDown;
    public Transform InteractionAnchor => anchor != null ? anchor : transform;
    public string Prompt => "Interactuar planta (F)";

    public Transform PosLeft => posLeft != null ? posLeft : transform;
    public Transform PosRight => posRight != null ? posRight : transform;
    public Transform PosUp => posUp != null ? posUp : transform;
    public Transform PosDown => posDown != null ? posDown : transform;

    public Plant plant;


    public void Awake()
    {
        plant = GetComponent<Plant>();
        if (plant == null)
            Debug.LogError("No hay planta en el interactable de planta");
    }

    public void Interact(Player player)
    {
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.CambiarPanel, "Panel Option Plant");
        //Debug.Log("Regaste la planta!");
        // lógica real
    }
}
