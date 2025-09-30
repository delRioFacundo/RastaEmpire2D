using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(RectTransform))]
public class DropBagUI : MonoBehaviour, IDropHandler
{
    [Header("Dónde parentear el bud embolsado")]
    public RectTransform content; // si es null, usa su propio RectTransform

    [Header("Eventos")]
    public UnityEvent onBagFilled;     // se dispara cuando entra el primer (y único) bud
    public UnityEvent onBagReject;     // se dispara si intentan meter otro y está llena

    RectTransform _rt;
    DraggableBudUI _current; // el bud que quedó dentro (si hay)

    void Awake()
    {
        _rt = GetComponent<RectTransform>();
        if (content == null) content = _rt;

        var img = GetComponent<Image>();
        if (img) img.raycastTarget = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        var go = eventData.pointerDrag;
        if (!go) return;

        var bud = go.GetComponent<DraggableBudUI>();
        if (bud == null || bud.IsBagged) return;

        // ¿Ya hay uno?
        if (_current != null)
        {
            onBagReject?.Invoke();     // opcional: shake/sonido
            bud.RevertToOrigin();      // vuelve a su lugar original
            return;
        }

        // Aceptamos: 1 por bolsa, posición SIEMPRE centro (0,0)
        bud.MarkBagged(content, Vector2.zero);
        _current = bud;
        onBagFilled?.Invoke();         // opcional: cerrar panel/cambiar estado
    }

    public bool IsFilled => _current != null;
    public DraggableBudUI CurrentBud => _current;
}
