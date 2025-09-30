using UnityEngine;

[CreateAssetMenu(menuName = "Rasta/Item")]
public class ItemSO : ScriptableObject
{
    public string itemId;          // único (ej: "weed_seed")
    public string displayName;
    public Sprite icon;
    public bool stackable = true;
    public int maxStack = 99;
}
