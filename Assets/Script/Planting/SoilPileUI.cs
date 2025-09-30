using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class SoilPileUI : MonoBehaviour, IPointerClickHandler
{
    public PotPlantUI pot;             // referencia a la maceta
    public float moveTime = 0.4f;
    public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Offset final al caer")]
    public Vector2 finalOffset;        // ajuste por montañita (ej: (-20, 10))

    RectTransform _rt;
    bool _used;

    void Awake()
    {
        _rt = GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_used) return;
        if (!pot || !pot.HasSeed) return;

        _used = true;
        StartCoroutine(MoveToCenter());
        pot.NotifyPileUsed();
    }

    IEnumerator MoveToCenter()
    {
        Vector2 start = _rt.anchoredPosition;
        Vector2 end = pot.GetHoleCenter() + finalOffset;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.01f, moveTime);
            float e = curve.Evaluate(t);
            _rt.anchoredPosition = Vector2.Lerp(start, end, e);
            yield return null;
        }

        _rt.anchoredPosition = end;
    }
}
