using UnityEngine;

public class DebugGiveItems : MonoBehaviour
{
    public Inventory inv;
    public ItemSO testItem;
    public int amount = 5;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            inv.Add(testItem, amount);
            Debug.Log("Added " + amount + " of " + testItem.displayName);
        }
    }
}
