using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] Inventory inventoryRef;
    [SerializeField] Transform slotHolder;   // tu GridLayoutGroup
    [SerializeField] InventorySlotUI slotPrefab;

    InventorySlotUI[] slotsUI;

    public Inventory InventoryRef => inventoryRef;

    void Start()
    {
        Build();
        inventoryRef.OnInventoryChanged += RefreshAll;
        RefreshAll();
    }

    void OnDestroy()
    {
        if (inventoryRef != null) inventoryRef.OnInventoryChanged -= RefreshAll;
    }

    void Build()
    {
        // limpiar anteriores
        foreach (Transform c in slotHolder) Destroy(c.gameObject);

        slotsUI = new InventorySlotUI[inventoryRef.Capacity];
        for (int i = 0; i < inventoryRef.Capacity; i++)
        {
            var ui = Instantiate(slotPrefab, slotHolder);
            ui.Setup(this, i);
            slotsUI[i] = ui;
        }
    }

    void RefreshAll()
    {
        for (int i = 0; i < slotsUI.Length; i++) slotsUI[i].Refresh();
    }
}
