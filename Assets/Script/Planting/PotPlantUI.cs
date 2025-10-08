using UnityEngine;
using UnityEngine.Events;

public class PotPlantUI : MonoBehaviour
{
    [Header("Referencias")]
    public DropSeedSlotUI holeSlot;   // el hueco
    public RectTransform holeCenter;  // para saber el centro exacto
    public SoilPileUI[] soilPiles;    // todas las montañitas alrededor

    [SerializeField] Plant plant;


    int _usedCount;

    public bool HasSeed => holeSlot && holeSlot.HasSeed;

    void Awake()
    {
        EventsManagerSpaar.SubscribeToEvent(EventTypeSpaar.UnsubscribeToEvent, UnsubscribeEvents);
        EventsManagerSpaar.SubscribeToEvent(EventTypeSpaar.PlantDetected, OnPlantDetected);
    }

    private void UnsubscribeEvents(object[] parameterContainer)
    {
        EventsManagerSpaar.UnsubscribeToEvent(EventTypeSpaar.UnsubscribeToEvent, UnsubscribeEvents);
        EventsManagerSpaar.UnsubscribeToEvent(EventTypeSpaar.PlantDetected, OnPlantDetected);
    }

    private void OnPlantDetected(object[] parameterContainer)
    {
        var p = (Plant)parameterContainer[0];
        plant = p;
    }

    public Vector2 GetHoleCenter()
    {
        if (holeCenter)
            return holeCenter.anchoredPosition;
        return Vector2.zero;
    }

    public void NotifyPileUsed()
    {
        _usedCount++;
        if (_usedCount >= soilPiles.Length)
        {
            EventsManagerSpaar.TriggerEvent(EventTypeSpaar.OpenPopup, "Planting");
            EventsManagerSpaar.TriggerEvent(EventTypeSpaar.ChangeStatePlayer, "Idle");
            EventsManagerSpaar.TriggerEvent(EventTypeSpaar.CambiarPanel, "Panel Game");
            plant.PlantSeed();
        }
    }
}
