using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class UIPopupAnimator : MonoBehaviour
{
    public enum Direction { Arriba, Abajo, Izquierda, Derecha }

    [Header("Configuración")]
    public Direction entrada = Direction.Abajo;
    public RectTransform panel;            // El panel que vas a animar
    public Vector2 posicionFinal = Vector2.zero; // Posición final en pantalla
    public float duracionEntrada = 0.6f;
    public float fuerzaRebote = 20f;       // Qué tan fuerte rebota
    public float duracionRebote = 0.15f;   // Duración de cada rebote

    Vector2 fueraPantalla;

    [SerializeField] string popupName;

    void Awake()
    {
        EventsManagerSpaar.SubscribeToEvent(EventTypeSpaar.OpenPopup, MostrarPopup);
        EventsManagerSpaar.SubscribeToEvent(EventTypeSpaar.ClosePopup, OcultarPopup);
        EventsManagerSpaar.SubscribeToEvent(EventTypeSpaar.UnsubscribeToEvent, UnsubscribeEvents);
    }

    private void UnsubscribeEvents(object[] parameterContainer)
    {
        EventsManagerSpaar.UnsubscribeToEvent(EventTypeSpaar.OpenPopup, MostrarPopup);
        EventsManagerSpaar.UnsubscribeToEvent(EventTypeSpaar.ClosePopup, OcultarPopup);
        EventsManagerSpaar.UnsubscribeToEvent(EventTypeSpaar.UnsubscribeToEvent, UnsubscribeEvents);
    }

    private void MostrarPopup(object[] parameterContainer)
    {
        var _popupName = (string)parameterContainer[0];
        if (_popupName == popupName)
        {
            MostrarPopup();
        }
    }

    private void OcultarPopup(object[] parameterContainer)
    {
        var _popupName = (string)parameterContainer[0];
        if (_popupName == popupName)
        {
            OcultarPopup();
        }
    }


    void Start()
    {
        if (!panel) panel = GetComponent<RectTransform>();
        CalcularPosicionInicial();
        panel.anchoredPosition = fueraPantalla;
    }

    [ContextMenu("Mostrar Popup")]
    public void MostrarPopup()
    {
        StopAllCoroutines();
        StartCoroutine(AnimarPopup());
    }

    [ContextMenu("Ocultar Popup")]
    public void OcultarPopup()
    {
        StopAllCoroutines();
        StartCoroutine(AnimarSalida());
    }

    void CalcularPosicionInicial()
    {
        Vector2 screenSize = panel.parent.GetComponent<RectTransform>().rect.size;
        switch (entrada)
        {
            case Direction.Arriba:
                fueraPantalla = new Vector2(posicionFinal.x, screenSize.y * 0.6f);
                break;
            case Direction.Abajo:
                fueraPantalla = new Vector2(posicionFinal.x, -screenSize.y * 0.6f);
                break;
            case Direction.Izquierda:
                fueraPantalla = new Vector2(-screenSize.x * 0.6f, posicionFinal.y);
                break;
            case Direction.Derecha:
                fueraPantalla = new Vector2(screenSize.x * 0.6f, posicionFinal.y);
                break;
        }
    }

    IEnumerator AnimarPopup()
    {
        CalcularPosicionInicial();
        panel.anchoredPosition = fueraPantalla;

        // Animación de entrada hasta la posición final
        float t = 0;
        Vector2 start = panel.anchoredPosition;

        while (t < 1f)
        {
            t += Time.deltaTime / duracionEntrada;
            panel.anchoredPosition = Vector2.Lerp(start, posicionFinal, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }

        // Rebote principal
        yield return StartCoroutine(Rebote(true));
    }

    IEnumerator AnimarSalida()
    {
        // Rebote antes de irse
        yield return StartCoroutine(Rebote(false));

        // Animación hacia fuera de pantalla
        float t = 0;
        Vector2 start = panel.anchoredPosition;
        CalcularPosicionInicial();
        Vector2 end = fueraPantalla;

        while (t < 1f)
        {
            t += Time.deltaTime / duracionEntrada;
            panel.anchoredPosition = Vector2.Lerp(start, end, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }

        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.CambiarPanel, "Panel Game");
    }

    IEnumerator Rebote(bool entrando)
    {
        Vector2 offset = Vector2.zero;

        switch (entrada)
        {
            case Direction.Arriba: offset = Vector2.down * fuerzaRebote; break;
            case Direction.Abajo: offset = Vector2.up * fuerzaRebote; break;
            case Direction.Izquierda: offset = Vector2.right * fuerzaRebote; break;
            case Direction.Derecha: offset = Vector2.left * fuerzaRebote; break;
        }

        Vector2 start = panel.anchoredPosition;
        Vector2 end = entrando ? (posicionFinal + offset) : (posicionFinal - offset);

        // Rebote ida
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / duracionRebote;
            panel.anchoredPosition = Vector2.Lerp(start, end, Mathf.Sin(t * Mathf.PI * 0.5f));
            yield return null;
        }

        // Rebote vuelta
        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / duracionRebote;
            panel.anchoredPosition = Vector2.Lerp(end, posicionFinal, Mathf.Sin(t * Mathf.PI * 0.5f));
            yield return null;
        }
    }
}
