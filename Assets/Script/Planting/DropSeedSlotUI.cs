using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(RectTransform))]
public class DropSeedSlotUI : MonoBehaviour, IDropHandler
{
    public UnityEvent onSeedPlaced;
    public UnityEvent onAlreadyHasSeed;

    RectTransform _rt;
    DraggableSeedUI _seed;

    void Awake()
    {
        _rt = GetComponent<RectTransform>();
        var img = GetComponent<Image>();
        if (img) img.raycastTarget = true; // recibe drop
    }

    public bool HasSeed => _seed != null;

    public void OnDrop(PointerEventData eventData)
    {
        var go = eventData.pointerDrag;
        if (!go) return;

        var seed = go.GetComponent<DraggableSeedUI>();
        if (seed == null || seed.IsPlaced) return;

        if (HasSeed)
        {
            onAlreadyHasSeed?.Invoke();
            seed.Revert();
            return;
        }

        seed.SnapTo(_rt, Vector2.zero);
        _seed = seed;
        onSeedPlaced?.Invoke();
    }
}
