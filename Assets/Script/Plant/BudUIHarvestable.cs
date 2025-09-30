using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class BudUIHarvestable : MonoBehaviour, IPointerClickHandler
{
    [Header("Animación simple")]
    public float fallDistance = 120f;   // px hacia abajo
    public float fallTime = 0.35f;      // seg
    public float rotateZ = 45f;         // grados
    public AnimationCurve curve = AnimationCurve.EaseInOut(0,0, 1,1);

    public bool harvested { get; private set; }

    RectTransform _rt;
    Image _img;

    PlantHarvestUI plantHarvestUI;

    void Awake()
    {
        _rt = GetComponent<RectTransform>();
        _img = GetComponent<Image>();
        plantHarvestUI = GetComponentInParent<PlantHarvestUI>();
        // Asegurate que el Image tenga Raycast Target activado.
        if (_img) _img.raycastTarget = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (harvested) return;
        harvested = true;
        StartCoroutine(HarvestAnim());
        print("Clickeado en " + name);
        // acá podrías notificar a la planta/inventario si querés
    }

    IEnumerator HarvestAnim()
    {
        Vector2 startPos = _rt.anchoredPosition;
        Quaternion startRot = _rt.localRotation;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.01f, fallTime);
            float e = curve.Evaluate(Mathf.Clamp01(t));
            _rt.anchoredPosition = startPos + Vector2.down * fallDistance * e;
            _rt.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, rotateZ, e));
            yield return null;
        }

        // Al terminar lo ocultamos (o Destroy si querés)
        gameObject.SetActive(false);
    }
}
