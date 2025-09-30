using System;
using UnityEngine;

[Serializable]
public class ItemStack
{
    public ItemSO item;
    public int amount;

    public ItemStack(ItemSO item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

    public bool IsEmpty => item == null || amount <= 0;
}
