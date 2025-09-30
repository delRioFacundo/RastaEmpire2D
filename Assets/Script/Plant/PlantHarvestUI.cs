using UnityEngine;
using UnityEngine.Events;

public class PlantHarvestUI : MonoBehaviour
{
    public BudUIHarvestable[] buds;

    public int TotalBuds => buds?.Length ?? 0;
    public int HarvestedCount
    {
        get
        {
            if (buds == null) return 0;
            int c = 0; foreach (var b in buds) if (b && b.harvested) c++;
            return c;
        }
    }
    public bool AllHarvested => HarvestedCount >= TotalBuds;

    void Awake()
    {
        if (buds == null || buds.Length == 0)
            buds = GetComponentsInChildren<BudUIHarvestable>(true);
    }

    void Update()
    {
        if (AllHarvested)
        {
            EventsManagerSpaar.TriggerEvent(EventTypeSpaar.HarvestComplete);
            EventsManagerSpaar.TriggerEvent(EventTypeSpaar.OpenPopup, "Harvest");
            enabled = false; // dejamos de chequear
            print("All buds harvested!");
        }
    }
}
