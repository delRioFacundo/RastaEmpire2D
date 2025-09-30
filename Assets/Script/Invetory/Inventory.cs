using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Tamaño del inventario")]
    [SerializeField] int capacity = 24;

    public List<ItemStack> slots;

    public System.Action OnInventoryChanged;

    void Awake()
    {
        slots = new List<ItemStack>(capacity);
        for (int i = 0; i < capacity; i++) slots.Add(new ItemStack(null, 0));
    }

    public bool Add(ItemSO item, int amount)
    {
        if (item == null || amount <= 0) return false;

        // 1) Intentar apilar en slots existentes
        if (item.stackable)
        {
            for (int i = 0; i < slots.Count && amount > 0; i++)
            {
                var s = slots[i];
                if (s.item == item && s.amount < item.maxStack)
                {
                    int space = item.maxStack - s.amount;
                    int toAdd = Mathf.Min(space, amount);
                    s.amount += toAdd;
                    amount -= toAdd;
                }
            }
        }

        // 2) Poner en slots vacíos
        for (int i = 0; i < slots.Count && amount > 0; i++)
        {
            var s = slots[i];
            if (s.IsEmpty)
            {
                int toAdd = item.stackable ? Mathf.Min(item.maxStack, amount) : 1;
                slots[i] = new ItemStack(item, toAdd);
                amount -= toAdd;
            }
        }

        bool changed = amount == 0;
        if (changed) OnInventoryChanged?.Invoke();
        return changed;
    }

    public void RemoveAt(int index, int amount = 9999)
    {
        if (index < 0 || index >= slots.Count) return;
        var s = slots[index];
        if (s.IsEmpty) return;

        s.amount -= amount;
        if (s.amount <= 0) slots[index] = new ItemStack(null, 0);
        OnInventoryChanged?.Invoke();
    }

    public int Capacity => slots.Count;
}
