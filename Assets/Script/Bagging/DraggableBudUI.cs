using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class DraggableBudUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool IsBagged { get; private set; }

    RectTransform _rt;
    Transform _originalParent;
    Vector2 _originalAnchoredPos;
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
        if (IsBagged) return;

        _originalParent = _rt.parent;
        _originalAnchoredPos = _rt.anchoredPosition;

        _rt.SetParent(_rootCanvas.transform, true);
        _cg.blocksRaycasts = false;
        _cg.alpha = 0.9f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsBagged) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rootCanvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out var localPoint);
        _rt.anchoredPosition = localPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsBagged) return;
        RevertToOrigin(); // si ninguna bolsa lo aceptó
    }

    public void RevertToOrigin()
    {
        _rt.SetParent(_originalParent, true);
        _rt.anchoredPosition = _originalAnchoredPos;
        _cg.blocksRaycasts = true;
        _cg.alpha = 1f;
    }

    // Llamado por la bolsa al aceptar el drop
    public void MarkBagged(Transform bagContent, Vector2 snapLocalPos)
    {
        IsBagged = true;
        _rt.SetParent(bagContent, true);
        _rt.anchoredPosition = snapLocalPos; // centro
        _cg.blocksRaycasts = true;
        _cg.alpha = 1f;

        // Si no querés que pueda volver a arrastrarse:
        enabled = false; // opcional
    }
}
