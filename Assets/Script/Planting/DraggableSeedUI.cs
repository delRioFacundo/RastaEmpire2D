using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class DraggableSeedUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool IsPlaced { get; private set; }

    RectTransform _rt;
    Transform _origParent;
    Vector2 _origPos;
    Canvas _rootCanvas;
    CanvasGroup _cg;

    void Awake()
    {
        _rt = GetComponent<RectTransform>();
        _rootCanvas = GetComponentInParent<Canvas>();
        _cg = GetComponent<CanvasGroup>();
        if (_cg == null) _cg = gameObject.AddComponent<CanvasGroup>();

        var img = GetComponent<Image>();
        if (img) img.raycastTarget = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsPlaced) return;

        _origParent = _rt.parent;
        _origPos = _rt.anchoredPosition;

        _rt.SetParent(_rootCanvas.transform, true);
        _cg.blocksRaycasts = false;
        _cg.alpha = 0.9f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsPlaced) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rootCanvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out var localPoint);
        _rt.anchoredPosition = localPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsPlaced) return;
        Revert();
    }

    public void Revert()
    {
        _rt.SetParent(_origParent, true);
        _rt.anchoredPosition = _origPos;
        _cg.blocksRaycasts = true;
        _cg.alpha = 1f;
    }

    // llamado por el slot cuando acepta el drop
    public void SnapTo(RectTransform parent, Vector2 localPos)
    {
        IsPlaced = true;
        _rt.SetParent(parent, true);
        _rt.anchoredPosition = localPos; // centrado
        _cg.blocksRaycasts = true;
        _cg.alpha = 1f;
        enabled = false; // ya no se arrastra más
    }
}
