using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI countText;

    int index;
    InventoryUI owner;

    public void Setup(InventoryUI owner, int index)
    {
        this.owner = owner;
        this.index = index;
        Refresh();
    }

    public void Refresh()
    {
        var stack = owner.InventoryRef.slots[index];
        bool hasItem = !stack.IsEmpty;

        icon.enabled = hasItem;
        icon.sprite = hasItem ? stack.item.icon : null;

        if (!hasItem) countText.text = "";
        else countText.text = stack.item.stackable && stack.amount > 1 ? stack.amount.ToString() : "";
    }

    // Ejemplo: click izquierdo para consumir uno
    public void OnClick()
    {
        owner.InventoryRef.RemoveAt(index, 1);
    }
}
