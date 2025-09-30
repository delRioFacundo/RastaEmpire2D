using UnityEngine;
using UnityEngine.Events;

public class PotPlantUI : MonoBehaviour
{
    [Header("Referencias")]
    public DropSeedSlotUI holeSlot;   // el hueco
    public RectTransform holeCenter;  // para saber el centro exacto
    public SoilPileUI[] soilPiles;    // todas las montañitas alrededor

   

    int _usedCount;

    public bool HasSeed => holeSlot && holeSlot.HasSeed;

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
        }
    }
}
